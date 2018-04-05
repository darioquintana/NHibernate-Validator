using System.Resources;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests
{
	public abstract class BaseValidatorFixture
	{
		public virtual IClassValidator GetClassValidator(System.Type type)
		{
			return new ClassValidator(type);
		}

		public virtual IClassValidator GetClassValidator(System.Type type, ResourceManager resource, CultureInfo culture)
		{
			return new ClassValidator(type, resource, culture, ValidatorMode.UseAttribute);
		}
	}
}
