using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV82
{
	public class DdlForComponents
	{
		protected ValidatorEngine ConfigureValidator(NHibernate.Cfg.Configuration configuration)
		{
			var nhvc = new FluentConfiguration();
			nhvc.SetDefaultValidatorMode(ValidatorMode.UseExternal).IntegrateWithNHibernate.ApplyingDDLConstraints();
			nhvc.Register<PersonValidation, Person>();
			nhvc.Register<NameValidation, Name>();
			var engine = new ValidatorEngine();
			engine.Configure(nhvc);
			return engine;
		}

		public NHibernate.Cfg.Configuration ConfigureNHibernate()
		{
			var cfg = new NHibernate.Cfg.Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			cfg.AddResource("NHibernate.Validator.Tests.Specifics.NHV82.Person.hbm.xml", Assembly.GetExecutingAssembly());
			return cfg;
		}

		[Test, Ignore("Not fixed yet")]
		public void ChangeLengthConstraintsForComponents()
		{
			var configuration = ConfigureNHibernate();
			var ve = ConfigureValidator(configuration);
			configuration.Initialize(ve);
			var cm = configuration.GetClassMapping(typeof (Person));
			var columns = cm.Table.ColumnIterator.ToArray();
			columns.First(c => c.Name == "FirstName").Length.Should().Be.EqualTo(20);
			columns.First(c => c.Name == "LastName").Length.Should().Be.EqualTo(35);
		}
	}
}