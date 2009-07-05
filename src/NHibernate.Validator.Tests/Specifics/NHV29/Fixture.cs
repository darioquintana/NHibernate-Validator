using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV29
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
			cfg.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Specifics.NHV29.Mappings.nhv.xml"));
			vtor.Configure(cfg);
			return vtor;
		}

		[Test]
		public void Should_Be_Able_To_Validate_A_Valid_Entity_With_Custom_Validator()
		{
			ValidatorEngine vtor = GetValidatorEngine();

			vtor.AssertValid(new Foo
			                 	{
			                 		Name = "Not null name"
			                 	});
		}

		[Test]
		public void Should_Be_Able_To_Validate_A_Invalid_Entity_With_Custom_Validator()
		{
			ValidatorEngine vtor = GetValidatorEngine();

			InvalidValue[] invalids = vtor.Validate(new Foo
			                                        	{
			                                        		Name = null
			                                        	});
			Assert.AreEqual(1, invalids.Length);
		}
	}
}