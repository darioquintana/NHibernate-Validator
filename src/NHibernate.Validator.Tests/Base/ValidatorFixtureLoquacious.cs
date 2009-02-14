using System.Reflection;
using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class ValidatorFixtureLoquacious : ValidatorFixture
	{
		private readonly ValidatorEngine ve;
		public ValidatorFixtureLoquacious()
		{
			var configure = new FluentConfiguration();
			configure.Register(
				Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.Namespace.Equals("NHibernate.Validator.Tests.Base"))
				.ValidationDefinitions())
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

		protected override bool AllowStaticFields
		{
			get { return false; }
		}

		protected override bool TestMessages
		{
			get { return false; }
		}
	}
}