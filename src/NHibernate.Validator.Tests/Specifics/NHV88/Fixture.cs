using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV88
{
	[TestFixture]
	public class Fixture
	{
		private ValidatorEngine validatorEngine;

		[TestFixtureSetUp]
		public void CreateValidatorEngine()
		{
			var configure = new FluentConfiguration();
			configure.Register(new[] {typeof (UserValidation), typeof (GroupValidation)})
				.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			validatorEngine = new ValidatorEngine();

			validatorEngine.Configure(configure);
		}

		[Test]
		public void GetInvalidValues_EntityWithInvalidProperty_ReturnsTheSameErrorsForEntityAsForProperty()
		{
			var group = new Group();
			var user = new User
			           	{
			           		Name = "qwerty",
			           		Group = group
			           	};


			InvalidValue[] userErrors = validatorEngine.Validate(user);

			userErrors.Count().Should().Be.EqualTo(1);

			InvalidValue[] userGroupErrors = validatorEngine.ValidatePropertyValue(user, "Group");

			userGroupErrors.Count().Should().Be.EqualTo(1);

			InvalidValue[] userNameErrors = validatorEngine.ValidatePropertyValue(user, "Name");

			userNameErrors.Count().Should().Be.EqualTo(0);
		}
	}
}