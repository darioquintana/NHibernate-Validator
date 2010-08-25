using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV95
{
	internal class FixtureNhv95
	{
		[Test,Ignore("Not fixed yet")]
		public void ValidateOnBaseClasses()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] {typeof (BaseEntityValidationDef)})
				.SetDefaultValidatorMode(ValidatorMode.UseExternal);

			var ve = new ValidatorEngine();
			ve.Configure(configure);

			ve.Validate(new Entity()).Any().Should().Be.True();
			//ve.GetClassValidator(typeof (Entity)).GetInvalidValues(new Entity()).Any().Should().Be.True();
		}

		public class BaseEntity
		{
			public string StringValue { get; set; }
		}

		public class BaseEntityValidationDef : ValidationDef<BaseEntity>
		{
			public BaseEntityValidationDef()
			{
				Define(x => x.StringValue).NotNullableAndNotEmpty();
			}
		}

		public class Entity : BaseEntity
		{
		}
	}
}