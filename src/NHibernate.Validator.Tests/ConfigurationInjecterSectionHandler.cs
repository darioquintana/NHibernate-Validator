using System.Configuration;
using System.Xml;
using NHibernate.Validator.Cfg;

namespace NHibernate.Validator.Tests
{
	public class ConfigurationInjecterSectionHandler : IConfigurationSectionHandler
	{
		private static INHVConfiguration cfgToInject;

		public static void SetCfgToInject(INHVConfiguration cfg)
		{
			cfgToInject = cfg;
		}

		public static void ResetCfgToInject()
		{
			cfgToInject = null;
		}

		#region IConfigurationSectionHandler Members

		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
		{
			if (cfgToInject == null)
			{
				XmlTextReader reader = new XmlTextReader(section.OuterXml, XmlNodeType.Document, null);
				return new NHVConfiguration(reader);
			}
			else
			{
				return cfgToInject;
			}
		}

		#endregion
	}
}
