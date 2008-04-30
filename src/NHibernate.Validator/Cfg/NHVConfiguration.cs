using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using log4net;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Configuration parsed values for nhv-configuration section.
	/// </summary>
	public class NHVConfiguration : INHVConfiguration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(NHVConfiguration));

		private const string CfgSchemaResource = "NHibernate.Validator.Cfg.nhv-configuration.xsd";
		private readonly XmlSchema config = ReadXmlSchemaFromEmbeddedResource(CfgSchemaResource);
		private string sharedEngineProviderClass;
		private readonly IList<MappingConfiguration> mappings = new List<MappingConfiguration>();
		private readonly IDictionary<string, string> properties = new Dictionary<string, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="NHVConfiguration"/> class.
		/// </summary>
		/// <remarks>An empty configuration.</remarks>
		public NHVConfiguration() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="NHVConfiguration"/> class.
		/// </summary>
		/// <param name="configurationReader">The XML reader to parse.</param>
		/// <remarks>
		/// The nhv-configuration.xsd is applied to the XML.
		/// </remarks>
		/// <exception cref="ValidatorConfigurationException">When nhibernate-configuration.xsd can't be applied.</exception>
		public NHVConfiguration(XmlReader configurationReader)
			: this(configurationReader, false) { }

		internal NHVConfiguration(XmlReader configurationReader, bool fromAppSetting)
		{
			XPathNavigator nav;
			try
			{
				nav = new XPathDocument(XmlReader.Create(configurationReader, GetSettings())).CreateNavigator();
			}
			catch (ValidatorConfigurationException)
			{
				throw;
			}
			catch (Exception e)
			{
				// Encapsule and reThrow
				throw new ValidatorConfigurationException(e);
			}
			Parse(nav, fromAppSetting);
		}

		#region INHVConfiguration Members

		public string SharedEngineProviderClass
		{
			get { return sharedEngineProviderClass; }
		}

		/// <summary>
		/// Configured properties
		/// </summary>
		public IDictionary<string, string> Properties
		{
			get { return properties; }
		}

		/// <summary>
		/// Configured Mappings
		/// </summary>
		public IList<MappingConfiguration> Mappings
		{
			get { return mappings; }
		}

		#endregion

		private XmlReaderSettings GetSettings()
		{
			XmlReaderSettings xmlrs = CreateConfigReaderSettings();
			return xmlrs;
		}

		private XmlReaderSettings CreateConfigReaderSettings()
		{
			XmlReaderSettings result = CreateXmlReaderSettings(config);
			result.ValidationEventHandler += ConfigSettingsValidationEventHandler;
			result.IgnoreComments = true;
			return result;
		}

		private static XmlReaderSettings CreateXmlReaderSettings(XmlSchema xmlSchema)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.Schema;
			settings.Schemas.Add(xmlSchema);
			return settings;
		}

		private static XmlSchema ReadXmlSchemaFromEmbeddedResource(string resourceName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();

			using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
			{
				return XmlSchema.Read(resourceStream, null);
			}
		}

		private static void ConfigSettingsValidationEventHandler(object sender, ValidationEventArgs e)
		{
			throw new ValidatorConfigurationException("An exception occurred parsing configuration :" + e.Message, e.Exception);
		}

		private void Parse(XPathNavigator navigator, bool fromAppSetting)
		{
			ParseSharedEngineProvider(navigator, fromAppSetting);
			ParseProperties(navigator);
			ParseMappings(navigator);
		}

		private void ParseSharedEngineProvider(XPathNavigator navigator, bool fromAppConfig)
		{
			XPathNavigator xpn = navigator.SelectSingleNode(CfgXmlHelper.SharedEngineClassExpression);
			if (xpn != null)
			{
				xpn.MoveToFirstAttribute();
				sharedEngineProviderClass = xpn.Value;

				if (!fromAppConfig && !string.IsNullOrEmpty(sharedEngineProviderClass))
				{
					log.Warn(
						string.Format("{0} propety is ignored out of application configuration file.", Environment.SharedEngineClass));
				}
			}
		}

		private void ParseProperties(XPathNavigator navigator)
		{
			XPathNodeIterator xpni = navigator.Select(CfgXmlHelper.PropertiesExpression);
			while (xpni.MoveNext())
			{
				string propName;
				string propValue = xpni.Current.Value;
				XPathNavigator pNav = xpni.Current.Clone();
				pNav.MoveToFirstAttribute();
				propName = pNav.Value;
				if (!string.IsNullOrEmpty(propName) && !string.IsNullOrEmpty(propValue))
				{
					properties[propName] = propValue;
				}
			}
		}

		private void ParseMappings(XPathNavigator navigator)
		{
			XPathNodeIterator xpni = navigator.Select(CfgXmlHelper.MappingsExpression);
			while (xpni.MoveNext())
			{
				MappingConfiguration mc = new MappingConfiguration(xpni.Current);
				if (!mc.IsEmpty())
				{
					if (!mappings.Contains(mc))
						mappings.Add(mc);
				}
			}
		}
	}
}