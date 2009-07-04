using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.ConstraintFactory.Model;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ConstraintFactory
{
	[TestFixture]
	public class ConstraintFactoryFixtureUsingAttributes
	{
		public virtual ValidatorEngine GetValidatorEngine()
		{
			var ve = new ValidatorEngine();
			var cfg = new XmlConfiguration();
			cfg.Properties[Environment.ValidatorMode] = "UseAttribute";
			cfg.Properties[Environment.ConstraintValidatorFactory] =
				typeof (TestConstraintValidatorFactory).AssemblyQualifiedName;
			ve.Configure(cfg);
			return ve;
		}

		[Test]
		public void Should_Create_Constraints_With_Custom_Factory()
		{
			TestConstraintValidatorFactory.ClearCounter();

			ValidatorEngine ve = GetValidatorEngine();
			ve.Validate(new Foo());

			Assert.AreEqual(3, TestConstraintValidatorFactory.ValidatorsCreated);
		}
	}

	public class ConstraintFactoryFixtureUsingXml : ConstraintFactoryFixtureUsingAttributes
	{
		public override ValidatorEngine GetValidatorEngine()
		{
			var ve = new ValidatorEngine();
			var cfg = new XmlConfiguration();
			cfg.Properties[Environment.ValidatorMode] = "UseExternal";
			cfg.Properties[Environment.ConstraintValidatorFactory] = 
				typeof (TestConstraintValidatorFactory).AssemblyQualifiedName;
			ve.Configure(cfg);
			return ve;
		}
	}

	public class ConstraintFactoryFixtureUsingFluent : ConstraintFactoryFixtureUsingAttributes
	{
		public override ValidatorEngine GetValidatorEngine()
		{
			var ve = new ValidatorEngine();
			var configuration = new FluentConfiguration();
			configuration
				.SetConstraintValidatorFactory<TestConstraintValidatorFactory>()
				.SetDefaultValidatorMode(ValidatorMode.UseExternal)
				.Register(new [] { typeof(Foo) }) ;
			
			ve.Configure(configuration);
			return ve;
		}
	}
}