using System.Reflection;
using NHibernate.Validator.Cfg;
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
			ve = new ValidatorEngine();
			var fml = new FluentMappingLoader();
			fml.AddNameSpace(Assembly.GetExecutingAssembly(), "NHibernate.Validator.Tests.Collections");
			var config = new NHVConfiguration();
			config.Properties[Environment.ValidatorMode] = "UseExternal";
			ve.Configure(config, fml);
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