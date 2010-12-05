using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using NHibernate.Validator.Cfg;

namespace NHibernate.Validator.Engine
{
	[Serializable]
	internal class StateFullClassValidatorFactory : AbstractClassValidatorFactory
	{
		private static readonly object syncRoot = new object();
		private static readonly IClassMappingFactory defaultClassMappingFactory = new JITClassMappingFactory();

		[NonSerialized] private IClassMappingFactory classMappingFactory = defaultClassMappingFactory;
		private readonly IDictionary<System.Type, IClassValidator> validators = new Dictionary<System.Type, IClassValidator>();

		public StateFullClassValidatorFactory(IConstraintValidatorFactory constraintValidatorFactory,
		                                      ResourceManager resourceManager, CultureInfo culture,
		                                      IMessageInterpolator userInterpolator, ValidatorMode validatorMode,
		                                      IEntityTypeInspector entityTypeInspector)
			: base(constraintValidatorFactory, resourceManager, culture, userInterpolator, validatorMode, entityTypeInspector) {}

		public void Initialize(IMappingLoader loader)
		{
			try
			{
				var sfcmf = new StateFullClassMappingFactory();
				classMappingFactory = sfcmf;
				var definitions = loader.GetMappings();
				foreach (var classMapping in definitions)
				{
					sfcmf.AddClassExternalDefinition(classMapping);
				}
				foreach (System.Type type in sfcmf.GetLoadedDefinitions())
				{
					GetRootValidator(type);
				}
			}
			finally
			{
				classMappingFactory = defaultClassMappingFactory;
			}
		}

		public override IClassMappingFactory ClassMappingFactory
		{
			get { return classMappingFactory; }
		}

		/// <summary>
		/// Get the validator for a type. If doesn't exists, will create one, add to the engine and return it.
		/// </summary>
		/// <param name="type">Type for the validator</param>
		/// <returns>Validator encountered or created-and-added.</returns>
		public override IClassValidator GetRootValidator(System.Type type)
		{
			lock (syncRoot)
			{
				IClassValidator result;
				if (!validators.TryGetValue(type, out result))
				{
					result = new ClassValidator(type, ConstraintValidatorFactory,  new Dictionary<System.Type, IClassValidator>(), this);
					validators.Add(type, result);
				}
				return result;
			}
		}

		public override void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType)
		{
				IClassValidator childValidator;
				if (validators.TryGetValue(childType, out childValidator))
				{
					lock (syncRoot)
					{
						parentValidator.ChildClassValidators.Add(childType, childValidator);
					}
				}
				else
				{
					new ClassValidator(childType, ConstraintValidatorFactory, parentValidator.ChildClassValidators, this);
				}
		}

		public IDictionary<System.Type, IClassValidator> Validators
		{
			get { return validators; }
		}

		[OnDeserialized]
		private void DeserializationCallBack(StreamingContext context)
		{
			classMappingFactory = defaultClassMappingFactory;
		}
	}
}
