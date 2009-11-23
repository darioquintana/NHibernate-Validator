using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV79
{
	[TestFixture]
	public class Fixture
	{
		[Test]
		public void can_validate_legs()
		{
			var validationDef = new ValidationDef<Cat>();
			validationDef.Define(c => c.Legs).GreaterThanOrEqualTo(2);

			var vc = new FluentConfiguration();
			vc.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			vc.Register(validationDef);

			var ve = new ValidatorEngine();
			ve.Configure(vc);

			ve.Validate(new Cat {Legs = 0}).Should().Have.Count.EqualTo(1);
			ve.Validate(new Cat {Legs = 3}).Should().Be.Empty();
		} 
	}
}
