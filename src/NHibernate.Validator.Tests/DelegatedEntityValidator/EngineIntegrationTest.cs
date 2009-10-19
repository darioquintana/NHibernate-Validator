using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.DelegatedEntityValidator
{
	[TestFixture]
	public class EngineIntegrationTest
	{
		[Test]
		public void Engine_Validate()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] {typeof (RangeDef)}).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(!ve.IsValid(new Range { Start = 5, End = 4 }));
			Assert.That(ve.IsValid(new Range { Start = 1, End = 4 }));
		}
	}
}