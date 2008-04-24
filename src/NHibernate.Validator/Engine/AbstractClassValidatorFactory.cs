using System.Globalization;
using System.Resources;

namespace NHibernate.Validator.Engine
{
	internal abstract class AbstractClassValidatorFactory : IClassValidatorFactory
	{
		private readonly ResourceManager resourceManager;
		private readonly CultureInfo culture;
		private readonly IMessageInterpolator userInterpolator;
		private readonly ValidatorMode validatorMode;

		public AbstractClassValidatorFactory(ResourceManager resourceManager, CultureInfo culture, IMessageInterpolator userInterpolator, ValidatorMode validatorMode)
		{
			this.resourceManager = resourceManager;
			this.culture = culture;
			this.userInterpolator = userInterpolator;
			this.validatorMode = validatorMode;
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
	}
}