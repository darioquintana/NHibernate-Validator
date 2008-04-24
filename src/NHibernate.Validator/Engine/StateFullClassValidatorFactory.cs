using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace NHibernate.Validator.Engine
{
	internal class StateFullClassValidatorFactory : AbstractClassValidatorFactory
	{
		private readonly Dictionary<System.Type, IClassValidator> validators = new Dictionary<System.Type, IClassValidator>();

		public StateFullClassValidatorFactory(ResourceManager resourceManager, CultureInfo culture, IMessageInterpolator userInterpolator, ValidatorMode validatorMode) 
			: base(resourceManager, culture, userInterpolator, validatorMode) {}

		public override IClassValidator GetRootValidator(System.Type type)
		{
			IClassValidator result;
			if(!validators.TryGetValue(type, out result))
			{
				result = new ClassValidator(type, new Dictionary<System.Type, IClassValidator>(), this);
				validators.Add(type, result);
			}
			return result;
		}

		public override void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType)
		{
			// TODO : Add an existing validators to child of the parent validator
			new ClassValidator(childType, parentValidator.ChildClassValidators, this);
		}
	}
}
