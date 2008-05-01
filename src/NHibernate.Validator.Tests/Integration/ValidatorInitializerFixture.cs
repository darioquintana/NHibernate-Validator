using System.Reflection;
using log4net.Config;
using log4net.Core;
using NHibernate.Validator.Cfg;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Integration
{
	[TestFixture]
	public class ValidatorInitializerFixture
	{
		[Test]
		public void WorkWithOutSharedEngine()
		{
			NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			string[] mappings =
				new string[]
					{
						"Integration.Address.hbm.xml",
						"Integration.Tv.hbm.xml",
						"Integration.TvOwner.hbm.xml",
						"Integration.Martian.hbm.xml",
						"Integration.Music.hbm.xml"
					};
			foreach (string file in mappings)
			{
				cfg.AddResource("NHibernate.Validator.Tests" + "." + file, Assembly.GetExecutingAssembly());
			}
			Environment.SharedEngineProvider = null;
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "true";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "UseAttribute";

			ValidatorInitializer.Initialize(cfg);
		}

		[Test]
		public void ApplyWrongConstraint()
		{
			XmlConfigurator.Configure();
			NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			cfg.AddResource("NHibernate.Validator.Tests.Integration.WrongClass.whbm.xml", Assembly.GetExecutingAssembly());
			Environment.SharedEngineProvider = null;
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "true";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "UseAttribute";

			using (LoggerSpy ls = new LoggerSpy(typeof(ValidatorInitializer), Level.Warn))
			{
				ValidatorInitializer.Initialize(cfg);
				int found =
					ls.GetOccurenceContaining(
						string.Format("Unable to apply constraints on DDL for [MappedClass={0}]", typeof (WrongClass).FullName));
				Assert.AreEqual(1, found);
				found =
					ls.GetOccurenceContaining(
						string.Format("Unable to apply constraints on DDL for [MappedClass={0}]", typeof (WrongClass1).FullName));
				Assert.AreEqual(1, found);
			}
		}
	}
}
