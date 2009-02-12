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
		private static readonly IClassMappingFactory defaultClassMappingFactory = new JITClassMappingFactory();

		[NonSerialized] private IClassMappingFactory classMappingFactory = defaultClassMappingFactory;
		private readonly IDictionary<System.Type, IClassValidator> validators = new Dictionary<System.Type, IClassValidator>();

		public StateFullClassValidatorFactory(ResourceManager resourceManager, CultureInfo culture, IMessageInterpolator userInterpolator, ValidatorMode validatorMode) 
			: base(resourceManager, culture, userInterpolator, validatorMode) {}

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
			lock (this)
			{
                IClassValidator result;
				if (!validators.TryGetValue(type, out result))
				{
					result = new ClassValidator(type, new Dictionary<System.Type, IClassValidator>(), this);
					validators.Add(type, result);
				}
				return result;
			}
		}

		public override void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType)
		{
			// TODO : Add an existing validators to child of the parent validator
			new ClassValidator(childType, parentValidator.ChildClassValidators, this);
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
