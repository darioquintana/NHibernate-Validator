using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

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

		[Test]
		public void DelegatedValidate_WithoutMessageNotThrow()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] { typeof(RangeDefWithoutCustomMessage) })
				.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);
			ActionAssert.NotThrow(()=>ve.IsValid(new Range { Start = 1, End = 4 }));
		}

		[Test]
		public void DelegatedValidate_WithoutMessageHasInvalidValue()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] { typeof(RangeDefWithoutCustomMessage) })
				.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);
			var iv = ve.Validate(new Range {Start = 5, End = 4});
			iv.Should().Not.Be.Empty();
		}
	}
}