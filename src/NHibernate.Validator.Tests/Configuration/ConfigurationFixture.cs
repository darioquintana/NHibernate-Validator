using System.Xml;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;
using System.Collections;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class ConfigurationFixture
	{
		[Test]
		public void MappingEquatable()
		{
			// Whole assembly and assembly&resource are equals
			Assert.IsTrue(
				(new MappingConfiguration("AAssembly", null)).Equals((new MappingConfiguration("AAssembly", "SomeResource"))),
				"Whole assembly is not equal then partial assembly.");

			// Two different partial assembly are not equals
			Assert.IsFalse(
				(new MappingConfiguration("AAssembly", "AnotherResource")).Equals(
					(new MappingConfiguration("AAssembly", "SomeResource"))));
			Assert.IsTrue((new MappingConfiguration("AAssembly", "")).Equals((new MappingConfiguration("AAssembly", null))));
			Assert.IsTrue(
				(new MappingConfiguration("AAssembly", "aResource")).Equals((new MappingConfiguration("AAssembly", "aResource"))));

			Assert.IsTrue((new MappingConfiguration("AFile")).Equals((new MappingConfiguration("AFile"))));
			Assert.IsFalse((new MappingConfiguration("AFile")).Equals((new MappingConfiguration("AnotherFile"))));
			Assert.IsFalse((new MappingConfiguration("AFile")).Equals((new MappingConfiguration("AAssembly", null))));
		}

		[Test]
		public void WellFormedConfiguration()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'>false</property>
		<property name='autoregister_listeners'>false</property>
		<property name='message_interpolator_class'>Myinterpolator</property>
		<property name='default-validator-mode'>OverrideAttributeWithXml</property>
		<mapping assembly='aAssembly'/>
		<mapping file='aFile'/>
		<mapping assembly='anotherAssembly' resource='TheResource'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			NHVConfiguration cfg = new NHVConfiguration(xtr);
			Assert.AreEqual(4, cfg.Properties.Count);
			Assert.AreEqual(3, cfg.Mappings.Count);
			Assert.AreEqual("false", cfg.Properties["apply_to_ddl"]);
			Assert.AreEqual("false", cfg.Properties["autoregister_listeners"]);
			Assert.AreEqual("Myinterpolator", cfg.Properties["message_interpolator_class"]);
			Assert.AreEqual("OverrideAttributeWithXml", cfg.Properties["default-validator-mode"]);
			Assert.Contains(new MappingConfiguration("aAssembly", ""), (IList)cfg.Mappings);
			Assert.Contains(new MappingConfiguration("aFile"), (IList)cfg.Mappings);
			Assert.Contains(new MappingConfiguration("anotherAssembly", "TheResource"), (IList)cfg.Mappings);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void BadMapping()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping file='aFile' assembly='aAssembly'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			new NHVConfiguration(xtr);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void BadSchema()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<unkonwednode/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			new NHVConfiguration(xtr);
		}

		[Test]
		public void NotDuplicatedItems()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'>false</property>
		<property name='apply_to_ddl'>true</property>
		<property name='default-validator-mode'>OverrideAttributeWithXml</property>
		<property name='default-validator-mode'>UseXml</property>
		<mapping assembly='aAssembly'/>
		<mapping assembly='aAssembly' resource='TheResource'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			NHVConfiguration cfg = new NHVConfiguration(xtr);
			Assert.AreEqual(2, cfg.Properties.Count);
			Assert.AreEqual(1, cfg.Mappings.Count);
			Assert.AreEqual("true", cfg.Properties["apply_to_ddl"]);
			Assert.AreEqual("UseXml", cfg.Properties["default-validator-mode"]);
			Assert.Contains(new MappingConfiguration("aAssembly", ""), (IList)cfg.Mappings);
		}

		[Test]
		public void IgnoreEmptyItems()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'></property>
		<property name='autoregister_listeners'></property>
		<property name='message_interpolator_class'></property>
		<property name='default-validator-mode'></property>
		<mapping assembly=''/>
		<mapping file=''/>
		<mapping assembly='' resource=''/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			NHVConfiguration cfg = new NHVConfiguration(xtr);
			Assert.AreEqual(0, cfg.Properties.Count);
			Assert.AreEqual(0, cfg.Mappings.Count);
		}

		[Test]
		public void IgnoreEmptyConfiguration()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			NHVConfiguration cfg = new NHVConfiguration(xtr);
			Assert.AreEqual(0, cfg.Properties.Count);
			Assert.AreEqual(0, cfg.Mappings.Count);
		}

	}
}
