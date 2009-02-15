using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Inheritance
{
	[TestFixture]
	public class LoquaciousInheritanceFixture : InheritanceFixture
	{
		private readonly ValidatorEngine ve;
		public LoquaciousInheritanceFixture()
		{
			var configure = new FluentConfiguration();
			configure.Register(
				Assembly.Load("NHibernate.Validator.Tests")
				.ValidationDefinitions()
				.Where(t => t.Namespace.Equals("NHibernate.Validator.Tests.Inheritance"))
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