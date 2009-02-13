using System;
using System.Linq;
using System.Reflection;
using System.Xml;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	[TestFixture]
	public class FluentMappingLoaderFixture
	{
		[Test]
		public void LoadMappingsNull()
		{
			var ml = new FluentMappingLoader();
			Assert.Throws<ArgumentNullException>(() => ml.LoadMappings(null));
		}

		[Test]
		public void LoadMappingsSpecific()
		{
			const string xmlConf =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Configuration.Loquacious.AddressValidationDef'/>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Configuration.Loquacious.BooValidationDef'/>
	</nhv-configuration>";
			var cfgXml = new XmlDocument();
			cfgXml.LoadXml(xmlConf);
			var xtr = new XmlTextReader(xmlConf, XmlNodeType.Document, null);
			var cfg = new NHVConfiguration(xtr);
			var ml = new FluentMappingLoader();
			ml.LoadMappings(cfg.Mappings);

			Assert.That(ml.GetMappings().Count(), Is.EqualTo(2));
		}

		[Test]
		public void LoadMappingsFromAssembly()
		{
			const string xmlConf =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests'/>
	</nhv-configuration>";
			var cfgXml = new XmlDocument();
			cfgXml.LoadXml(xmlConf);
			var xtr = new XmlTextReader(xmlConf, XmlNodeType.Document, null);
			var cfg = new NHVConfiguration(xtr);
			var ml = new FluentMappingLoader();
			ml.LoadMappings(cfg.Mappings);

			Assert.That(ml.GetMappings().Count(), Is.GreaterThan(1)); // the mappings of tests are more than 1 ;)
		}

		[Test]
		public void ClassNotFound()
		{
			const string xml = @"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='Base.Address.nhv.xml'/>
	</nhv-configuration>";
			var cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			var xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			var cfg = new NHVConfiguration(xtr);
			var ml = new FluentMappingLoader();
			Assert.Throws<ValidatorConfigurationException>(() => ml.LoadMappings(cfg.Mappings));
		}

		[Test]
		public void AddWrongAssembly()
		{
			var ml = new FluentMappingLoader();
			Assert.Throws<ValidatorConfigurationException>(() => ml.AddAssembly("NoExistAssemblyName"));
		}

		[Test]
		public void AddAssembly()
		{
			// in this test we only try to load an entirely assembly without check what was load
			var ml = new FluentMappingLoader();
			ml.AddAssembly(Assembly.GetExecutingAssembly());
			Assert.That(ml.GetMappings().Count(), Is.GreaterThan(1));
		}

		[Test]
		public void AddAssemblyByName()
		{
			// in this test we only try to load an entirely assembly without check what was load
			var ml = new FluentMappingLoader();
			ml.AddAssembly(Assembly.GetExecutingAssembly().FullName);
			Assert.That(ml.GetMappings().Count(), Is.GreaterThan(1));
		}

		[Test] 
		public void AddWrongClassDefinition()
		{
			var ml = new FluentMappingLoader();
			Assert.Throws<ValidatorConfigurationException>(() => ml.AddClassDefinition(Assembly.GetExecutingAssembly(), "ClassDefinition"));
		}

		[Test]
		public void AddTypedClassDefinition()
		{
			var ml = new FluentMappingLoader();
			Assert.Throws<ArgumentNullException>(() => ml.AddClassDefinition<AddressValidationDef>(null));
			ml.AddClassDefinition(new AddressValidationDef());
			Assert.That(ml.GetMappings().Count(), Is.EqualTo(1));
		}

		[Test]
		public void AddClassDefinitionByType()
		{
			var ml = new FluentMappingLoader();
			ml.AddClassDefinition<AddressValidationDef, Address>();
			Assert.That(ml.GetMappings().Count(), Is.EqualTo(1));
		}
	}
}