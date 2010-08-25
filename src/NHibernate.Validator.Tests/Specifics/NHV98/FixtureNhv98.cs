using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV98
{
	public class FixtureNhv98
	{
		[Test]
		public void MaxLengthWorks()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] {typeof (EntityValidator)})
				.SetDefaultValidatorMode(ValidatorMode.UseExternal);

			var entity = new TwoRuleEntity
			             	{
			             		SomeString = string.Empty.PadLeft(10).Replace(" ", "A")
			             	};

			//passes
			entity.SomeString.Length.Should().Be.GreaterThan(5);

			var ve = new ValidatorEngine();
			ve.Configure(configure);

			ve.Validate(entity).Any().Should().Be.True();
		}

		#region Nested type: EntityValidator

		public class EntityValidator : ValidationDef<TwoRuleEntity>
		{
			public EntityValidator()
			{
				Define(r => r.SomeString)
					.LengthBetween(1,5)
					.WithMessage("Error: Can't be less of 1 or over 5 characters long");
			}
		}

		#endregion

		#region Nested type: TwoRuleEntity

		public class TwoRuleEntity
		{
			public virtual string SomeString { get; set; }
		}

		#endregion
	}
}