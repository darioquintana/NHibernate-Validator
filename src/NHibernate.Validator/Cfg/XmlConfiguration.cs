using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Configuration parsed values for nhv-configuration section.
	/// </summary>
	public class XmlConfiguration : INHVConfiguration
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (XmlConfiguration));

		private const string CfgSchemaResource = "NHibernate.Validator.Cfg.nhv-configuration.xsd";
		private readonly XmlSchema config = ReadXmlSchemaFromEmbeddedResource(CfgSchemaResource);
		private readonly HashSet<System.Type> entityTypeInspectors = new HashSet<System.Type>();

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlConfiguration"/> class.
		/// </summary>
		/// <remarks>An empty configuration.</remarks>
		public XmlConfiguration()
		{
			entityTypeInspectors = new HashSet<System.Type>();
			Mappings = new List<MappingConfiguration>();
			Properties = new Dictionary<string, string>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlConfiguration"/> class.
		/// </summary>
		/// <param name="configurationReader">The XML reader to parse.</param>
		/// <remarks>
		/// The nhv-configuration.xsd is applied to the XML.
		/// </remarks>
		/// <exception cref="ValidatorConfigurationException">When nhibernate-configuration.xsd can't be applied.</exception>
		public XmlConfiguration(XmlReader configurationReader) : this(configurationReader, false) {}

		internal XmlConfiguration(XmlReader configurationReader, bool fromAppSetting):this()
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

		public string SharedEngineProviderClass { get; private set; }

		/// <summary>
		/// Configured properties
		/// </summary>
		public IDictionary<string, string> Properties { get; private set; }

		/// <summary>
		/// Configured Mappings
		/// </summary>
		public IList<MappingConfiguration> Mappings { get; private set; }

		public IEnumerable<System.Type> EntityTypeInspectors
		{
			get { return entityTypeInspectors; }
		}

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
			var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
			settings.Schemas.Add(xmlSchema);
			return settings;
		}

		private static XmlSchema ReadXmlSchemaFromEmbeddedResource(string resourceName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();

			using (Stream resourceStream = executingAssembly.GetManifestResourceStream(resourceName))
			{
				if (resourceStream == null)
				{
					throw new ArgumentException("No resources were specified during compilation, or if the resource is not visible.",
					                            "resourceName");
				}
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
			ParseEntityTypeInspectors(navigator);
		}

		private void ParseSharedEngineProvider(XPathNavigator navigator, bool fromAppConfig)
		{
			XPathNavigator xpn = navigator.SelectSingleNode(CfgXmlHelper.SharedEngineClassExpression);
			if (xpn != null)
			{
				xpn.MoveToFirstAttribute();
				SharedEngineProviderClass = xpn.Value;

				if (!fromAppConfig && !string.IsNullOrEmpty(SharedEngineProviderClass))
				{
					log.Warn(string.Format("{0} propety is ignored out of application configuration file.",
					                       Environment.SharedEngineClass));
				}
			}
		}

		private void ParseProperties(XPathNavigator navigator)
		{
			XPathNodeIterator xpni = navigator.Select(CfgXmlHelper.PropertiesExpression);
			while (xpni.MoveNext())
			{
				string propValue = xpni.Current.Value;
				XPathNavigator pNav = xpni.Current.Clone();
				pNav.MoveToFirstAttribute();
				string propName = pNav.Value;
				if (!string.IsNullOrEmpty(propName) && !string.IsNullOrEmpty(propValue))
				{
					Properties[propName] = propValue;
				}
			}
		}

		private void ParseMappings(XPathNavigator navigator)
		{
			XPathNodeIterator xpni = navigator.Select(CfgXmlHelper.MappingsExpression);
			while (xpni.MoveNext())
			{
				var mc = new MappingConfiguration(xpni.Current);
				if (!mc.IsEmpty())
				{
					if (!Mappings.Contains(mc))
					{
						Mappings.Add(mc);
					}
				}
			}
		}

		private void ParseEntityTypeInspectors(XPathNavigator navigator)
		{
			XPathNodeIterator xpni = navigator.Select(CfgXmlHelper.EntityTypeInspectorsExpression);
			while (xpni.MoveNext())
			{
				XPathNavigator pNav = xpni.Current.Clone();
				pNav.MoveToFirstAttribute();
				string fullName = pNav.Value;
				if (!string.IsNullOrEmpty(fullName))
				{
					entityTypeInspectors.Add(System.Type.GetType(fullName));
				}
			}
		}
	}
}