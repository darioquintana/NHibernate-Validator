using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class XmlMappingLoaderFixture
	{
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void LoadMappingsNull()
		{
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.LoadMappings(null);
		}

		[Test]
		public void LoadMappingsTest()
		{
			string xml =
	@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Base.Address.nhv.xml'/>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Base.Boo.nhv.xml'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.LoadMappings(cfg.Mappings);
			Assert.AreEqual(2, ml.Mappings.Length);

			xml =
@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests'/>
	</nhv-configuration>";
			cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			cfg = new XmlConfiguration(xtr);
			ml = new XmlMappingLoader();
			ml.LoadMappings(cfg.Mappings);
			Assert.Less(1, ml.Mappings.Length); // the mappings of tests are more than 1 ;)

			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<class name='Boo'>");
				sw.WriteLine("<property name='field'><notnullorempty/></property>");
				sw.WriteLine("</class>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}
			xml = string.Format(
@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping file='{0}'/>
	</nhv-configuration>", tmpf);
			cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			cfg = new XmlConfiguration(xtr);
			ml = new XmlMappingLoader();
			ml.LoadMappings(cfg.Mappings);
			Assert.AreEqual(1, ml.Mappings.Length);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void ResourceNotFound()
		{
			string xml =
	@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='Base.Address.nhv.xml'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.LoadMappings(cfg.Mappings);
		}

		[Test]
		public void AddStream()
		{
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<class name='Boo'>");
				sw.WriteLine("<property name='field'><notnullorempty/></property>");
				sw.WriteLine("</class>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}

			XmlMappingLoader ml = new XmlMappingLoader();
			using (StreamReader sr = new StreamReader(tmpf))
			{
				ml.AddInputStream(sr.BaseStream, tmpf);
			}
			Assert.AreEqual(1, ml.Mappings.Length);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void AddWrongStream()
		{
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<no valid node>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}

			XmlMappingLoader ml = new XmlMappingLoader();
			using (StreamReader sr = new StreamReader(tmpf))
			{
				ml.AddInputStream(sr.BaseStream, tmpf);
			}
			Assert.AreEqual(1, ml.Mappings.Length);
		}

		[Test]
		public void AddFile()
		{
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<class name='Boo'>");
				sw.WriteLine("<property name='field'><notnullorempty/></property>");
				sw.WriteLine("</class>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.AddFile(tmpf);
			Assert.AreEqual(1, ml.Mappings.Length);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void AddWrongFile()
		{
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<no valid node>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.AddFile(tmpf);
		}

		[Test]
		public void AddWrongFileName()
		{
			XmlMappingLoader ml = new XmlMappingLoader();
			Assert.Throws<ValidatorConfigurationException>(() => ml.AddFile("NoExistFile"),"Could not load file NoExistFile");
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void AddWrongAssembly()
		{
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.AddAssembly("NoExistAssemblyName");
		}

		[Test]
		public void AddAssembly()
		{
			// in this test we only try to load an entirely assembly without check what was load
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.AddAssembly(Assembly.GetExecutingAssembly());
			Assert.Less(0, ml.Mappings.Length);
		}

		[Test]
		public void AddAssemblyByName()
		{
			// in this test we only try to load an entirely assembly without check what was load
			XmlMappingLoader ml = new XmlMappingLoader();
			ml.AddAssembly(Assembly.GetExecutingAssembly().FullName);
			Assert.Less(0, ml.Mappings.Length);
		}

		[Test]
		public void MixingLoaders()
		{
			string xml =
@"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Base.Address.nhv.xml'/>
	</nhv-configuration>";
			XmlDocument cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			XmlConfiguration cfg = new XmlConfiguration(xtr);
			XmlMappingLoader ml = new XmlMappingLoader();

			ml.LoadMappings(cfg.Mappings);

			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'>");
				sw.WriteLine("<class name='Boo'>");
				sw.WriteLine("<property name='field'><notnullorempty/></property>");
				sw.WriteLine("</class>");
				sw.WriteLine("</nhv-mapping>");
				sw.Flush();
			}
			ml.AddFile(tmpf);
			Assert.AreEqual(2, ml.Mappings.Length);
		}

		[Test]
		public void GetMappingForType()
		{
			NhvMapping mapping = XmlMappingLoader.GetXmlMappingFor(typeof(Base.Address));
			Assert.IsNotNull(mapping);
			Assert.IsTrue(mapping == mapping.@class[0].rootMapping);
			Assert.IsNull(XmlMappingLoader.GetXmlMappingFor(typeof(Base.Building)));
		}

		[Test]
		public void CompiledMappings()
		{
			const string xml = @"<nhv-configuration xmlns='urn:nhv-configuration-1.0'>
		<mapping assembly='NHibernate.Validator.Tests' resource='NHibernate.Validator.Tests.Base.Address.nhv.xml'/>
	</nhv-configuration>";
			var cfgXml = new XmlDocument();
			cfgXml.LoadXml(xml);
			var xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
			var cfg = new XmlConfiguration(xtr);
			var ml = new XmlMappingLoader();

			ml.LoadMappings(cfg.Mappings);
			var cm = ml.GetMappings();
			Assert.That(cm.Count(), Is.EqualTo(1));
			Assert.That(cm.FirstOrDefault(x => x.EntityType == typeof(Base.Address)), Is.Not.Null);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void MappingDocumentParserTest()
		{
			// here we test only the exception since the other tests are included in MappingLoader
			MappingDocumentParser mdp = new MappingDocumentParser();
			mdp.Parse(null);
		}
	}
}
