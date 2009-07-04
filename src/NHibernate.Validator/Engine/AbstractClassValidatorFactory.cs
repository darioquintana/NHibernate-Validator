using System.Globalization;
using System.Resources;
using System;

namespace NHibernate.Validator.Engine
{
	[Serializable]
	internal abstract class AbstractClassValidatorFactory : IClassValidatorFactory
	{
		private readonly ResourceManager resourceManager;
		private readonly CultureInfo culture;
		private readonly IMessageInterpolator userInterpolator;
		private readonly ValidatorMode validatorMode;
		private readonly IConstraintValidatorFactory constraintValidatorFactory;
		
		public AbstractClassValidatorFactory(IConstraintValidatorFactory constraintValidatorFactory, ResourceManager resourceManager, CultureInfo culture, IMessageInterpolator userInterpolator, ValidatorMode validatorMode)
		{
			this.constraintValidatorFactory = constraintValidatorFactory;
			this.resourceManager = resourceManager;
			this.culture = culture;
			this.userInterpolator = userInterpolator;
			this.validatorMode = validatorMode;
		}

		public IConstraintValidatorFactory ConstraintValidatorFactory
		{
			get { return constraintValidatorFactory; }
		}

		public ResourceManager ResourceManager
		{
			get { return resourceManager; }
		}

		public CultureInfo Culture
		{
			get { return culture; }
		}

		public IMessageInterpolator UserInterpolator
		{
			get { return userInterpolator; }
		}

		public ValidatorMode ValidatorMode
		{
			get { return validatorMode; }
		}

		public abstract IClassValidator GetRootValidator(System.Type type);

		public abstract void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType);

		public abstract IClassMappingFactory ClassMappingFactory { get; }
	}
}