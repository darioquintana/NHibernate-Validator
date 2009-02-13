using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture, Ignore("Not fully checked")]
	public class ValidatorFixtureLoquacious : ValidatorFixture
	{
		private readonly ValidatorEngine ve;
		public ValidatorFixtureLoquacious()
		{
			ve = new ValidatorEngine();
			var fml = new FluentMappingLoader();
			fml.AddNameSpace(Assembly.GetExecutingAssembly(), "NHibernate.Validator.Tests.Base");
			var config = new NHVConfiguration();
			config.Properties[Environment.ValidatorMode] = "UseExternal";
			ve.Configure(config, fml);
		}

		public override IClassValidator GetClassValidator(System.Type type)
		{
			return ve.GetClassValidator(type);
		}
	}
}