using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Collections
{
	[TestFixture]
	public class CollectionFixtureLoquacious : CollectionFixture
	{
		private readonly ValidatorEngine ve;
		public CollectionFixtureLoquacious()
		{
			var configure = new FluentConfiguration();
			configure.Register(
				Assembly.GetExecutingAssembly().GetTypes()
				.ValidationDefinitions()
				.Where(t => t.Namespace.Equals("NHibernate.Validator.Tests.Collections"))
				)
			.SetDefaultValidatorMode(ValidatorMode.UseExternal);

			ve = new ValidatorEngine();
			ve.Configure(configure);
		}

		public override IClassValidator GetClassValidator(System.Type type)
		{
			return ve.GetClassValidator(type);
		}

		public override IClassValidator GetClassValidator(System.Type type, System.Resources.ResourceManager resource, System.Globalization.CultureInfo culture)
		{
			return ve.GetClassValidator(type);
		}
	}
}