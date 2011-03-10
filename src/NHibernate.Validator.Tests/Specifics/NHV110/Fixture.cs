using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV110
{
	[TestFixture]
	public class Fixture
	{
		public ValidatorEngine GetValidatorEngine()
		{
			var vtor = new ValidatorEngine();
			var cfg = new XmlConfiguration();
			cfg.Properties[Environment.ValidatorMode] = "UseExternal";
			string an = Assembly.GetExecutingAssembly().FullName;
			cfg.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Specifics.NHV110.Mappings.nhv.xml"));
			vtor.Configure(cfg);
			return vtor;
		}

		[Test]
		public void Should_Be_Able_To_Validate_A_Valid_Entity_With_Custom_Validator()
		{
			ValidatorEngine vtor = GetValidatorEngine();
			vtor.Validate(new FooEntityValidator()).Length.Should().Be.EqualTo(1);
		}

		[Test]
		public void Should_Be_Able_To_Validate_A_Valid_Property_With_Custom_Validator()
		{
			ValidatorEngine vtor = GetValidatorEngine();
			vtor.Validate(new FooPropertyValidator()).Length.Should().Be.EqualTo(1);
		}
	}
}