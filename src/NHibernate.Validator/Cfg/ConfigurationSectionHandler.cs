using System.Configuration;
using System.Xml;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Readonly NHV configuration section handler
	/// </summary>
	public class ConfigurationSectionHandler : IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler Members

		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
		{
			XmlTextReader reader = new XmlTextReader(section.OuterXml, XmlNodeType.Document, null);
			return new XmlConfiguration(reader, true);
		}

		#endregion
	}
}