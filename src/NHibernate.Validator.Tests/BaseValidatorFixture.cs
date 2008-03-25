using log4net.Config;
using System.Resources;
using System.Globalization;

namespace NHibernate.Validator.Tests
{
	public abstract class BaseValidatorFixture
	{
		static BaseValidatorFixture()
		{
			XmlConfigurator.Configure();
		}

		public virtual ClassValidator GetClassValidator(System.Type type)
		{
			return new ClassValidator(type);
		}

		public virtual ClassValidator GetClassValidator(System.Type type, ResourceManager resource, CultureInfo culture)
		{
			return new ClassValidator(type, resource, culture, ValidatorMode.UseAttribute);
		}
	}
}
