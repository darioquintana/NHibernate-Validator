using System;
using System.Linq;
using System.Xml;
using log4net.Config;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;
using System.Collections;
using log4net.Core;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class XmlConfigurationFixture
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
			XmlConfigurator.Configure();

			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'>false</property>
		<property name='autoregister_listeners'>false</property>
		<property name='message_interpolator_class'>Myinterpolator</property>
		<property name='external_mappings_loader_class'>MyLoader</property>
		<property name='default_validator_mode'>OverrideAttributeWithXml</property>
		<property name='constraint_validator_factory'>SomeNamespace.Type,SomeAssembly</property>
		<mapping assembly='aAssembly'/>
		<mapping file='aFile'/>
		<mapping assembly='anotherAssembly' resource='TheResource'/>
		<shared_engine_provider class='MySharedEngineProvider'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg;
			using (LoggerSpy ls = new LoggerSpy(typeof(XmlConfiguration), Level.Warn))
			{
				cfg = new XmlConfiguration(xtr);
				int found = ls.GetOccurenceContaining(NHibernate.Validator.Cfg.Environment.SharedEngineClass + " propety is ignored out of application configuration file.");
				Assert.AreEqual(1, found);
			}
			Assert.AreEqual("MySharedEngineProvider", cfg.SharedEngineProviderClass);
			Assert.AreEqual(6, cfg.Properties.Count);
			Assert.AreEqual(3, cfg.Mappings.Count);
			Assert.AreEqual("false", cfg.Properties["apply_to_ddl"]);
			Assert.AreEqual("false", cfg.Properties["autoregister_listeners"]);
			Assert.AreEqual("Myinterpolator", cfg.Properties["message_interpolator_class"]);
			Assert.AreEqual("MyLoader", cfg.Properties["external_mappings_loader_class"]);
			Assert.AreEqual("OverrideAttributeWithXml", cfg.Properties["default_validator_mode"]);
			Assert.AreEqual("SomeNamespace.Type,SomeAssembly",cfg.Properties["constraint_validator_factory"]);
			Assert.Contains(new MappingConfiguration("aAssembly", ""), (IList)cfg.Mappings);
			Assert.Contains(new MappingConfiguration("aFile"), (IList)cfg.Mappings);
			Assert.Contains(new MappingConfiguration("anotherAssembly", "TheResource"), (IList)cfg.Mappings);
		}

		[Test]
		public void BadMapping()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping file='aFile' assembly='aAssembly'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			ActionAssert.Throws<ValidatorConfigurationException>(() => new XmlConfiguration(xtr));
		}

		[Test]
		public void NullReader()
		{
			ActionAssert.Throws<ValidatorConfigurationException>(() => new XmlConfiguration(null));
		}

		[Test]
		public void BadSchema()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<unkonwednode/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			ActionAssert.Throws<ValidatorConfigurationException>(() =>new XmlConfiguration(xtr));
		}

		[Test]
		public void NotDuplicatedItems()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'>false</property>
		<property name='apply_to_ddl'>true</property>
		<property name='default_validator_mode'>OverrideAttributeWithXml</property>
		<property name='default_validator_mode'>UseXml</property>
		<mapping assembly='aAssembly'/>
		<mapping assembly='aAssembly' resource='TheResource'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			Assert.AreEqual(2, cfg.Properties.Count);
			Assert.AreEqual(1, cfg.Mappings.Count);
			Assert.AreEqual("true", cfg.Properties["apply_to_ddl"]);
			Assert.AreEqual("UseXml", cfg.Properties["default_validator_mode"]);
			Assert.Contains(new MappingConfiguration("aAssembly", ""), (IList)cfg.Mappings);

			// Accept only MappingConfiguration object for Equals comparison
			Assert.IsFalse((new MappingConfiguration("NHibernate.Validator.Tests", "")).Equals("NHibernate.Validator.Tests"));
			Assert.IsFalse((new MappingConfiguration("NHibernate.Validator.Tests", "")).Equals(null));
		}

		[Test]
		public void IgnoreEmptyItems()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='apply_to_ddl'></property>
		<property name='autoregister_listeners'></property>
		<property name='message_interpolator_class'></property>
		<property name='default_validator_mode'></property>
		<mapping assembly=''/>
		<mapping file=''/>
		<mapping assembly='' resource=''/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
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
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			Assert.AreEqual(0, cfg.Properties.Count);
			Assert.AreEqual(0, cfg.Mappings.Count);
		}

		[Test]
		public void MappingConfigurationCtors()
		{
			try
			{
				new MappingConfiguration("", "");
				Assert.Fail("Constructor accept empty assembly name");
			}
			catch(ArgumentException)
			{
				//ok
			}
			try
			{
				new MappingConfiguration("");
				Assert.Fail("Constructor accept empty file name");
			}
			catch (ArgumentException)
			{
				//ok
			}
		}

		[Test]
		public void MappingConfigurationToString()
		{
			Assert.AreEqual("file='';assembly='NHibernate.Validator.Tests';resource=''", (new MappingConfiguration("NHibernate.Validator.Tests", "")).ToString());
			Assert.AreEqual("file='aFilePath';assembly='';resource=''", (new MappingConfiguration("aFilePath")).ToString());
			Assert.AreEqual("file='';assembly='NHibernate.Validator.Tests';resource='AResource'", (new MappingConfiguration("NHibernate.Validator.Tests", "AResource")).ToString());
		}

		[Test]
		public void IgnoreEmptyItemsEntityTypeInspectors()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<entity-type-inspector class=''/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			Assert.That(cfg.EntityTypeInspectors, Is.Empty);
		}

		[Test]
		public void CanReadEntityTypeInspectors()
		{
			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<entity-type-inspector class='NHibernate.Validator.Engine.DefaultEntityTypeInspector, NHibernate.Validator'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			Assert.That(cfg.EntityTypeInspectors, Is.Not.Empty);
			Assert.That(cfg.EntityTypeInspectors.First(), Is.EqualTo(typeof(Validator.Engine.DefaultEntityTypeInspector)));
		}

		[Test]
		public void CanReadCustomResourceManager()
		{
			XmlConfigurator.Configure();

			string xml =
				@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<property name='resource_manager'>YourFullNameSpace.TheBaseNameOfTheResourceFileWithoutExtensionNorCulture, YourAssembly</property>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			Assert.That(cfg.Properties["resource_manager"], Is.EqualTo("YourFullNameSpace.TheBaseNameOfTheResourceFileWithoutExtensionNorCulture, YourAssembly"));
		}
	}
}
