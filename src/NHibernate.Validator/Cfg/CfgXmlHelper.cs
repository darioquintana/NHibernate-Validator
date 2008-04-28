using System.Xml;
using System.Xml.XPath;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Helper to parse nhv-configuration XmlNode.
	/// </summary>
	public static class CfgXmlHelper
	{
		/// <summary>
		/// The XML node name for hibernate configuration section in the App.config/Web.config and
		/// for the hibernate.cfg.xml .
		/// </summary>
		public const string CfgSectionName = "nhv-configuration";
		public const string DefaultNHVCfgFileName = "nhvalidator.cfg.xml";

		/// <summary>The XML Namespace for the nhibernate-configuration</summary>
		public const string CfgSchemaXMLNS = "urn:nhv-configuration-1.0";

		// note that the prefix has absolutely nothing to do with what the user
		// selects as their prefix in the document. It is the prefix we use to 
		// build the XPath and the nsMgr takes care of translating our prefix into
		// the user defined prefix...
		public const string CfgNamespacePrefix = "cfg";
		private const string RootPrefixPath = "//" + CfgNamespacePrefix + ":";

		private static readonly XmlNamespaceManager nsMgr;

		static CfgXmlHelper()
		{
			NameTable nt = new NameTable();
			nsMgr = new XmlNamespaceManager(nt);
			nsMgr.AddNamespace(CfgNamespacePrefix, CfgSchemaXMLNS);

			PropertiesExpression = XPathExpression.Compile(RootPrefixPath + "property", nsMgr);
			MappingsExpression = XPathExpression.Compile(RootPrefixPath + "mapping", nsMgr);
		}

		/// <summary>XPath expression for property nodes</summary>
		public static readonly XPathExpression PropertiesExpression;
		/// <summary>XPath expression for mapping nodes</summary>
		public static readonly XPathExpression MappingsExpression;

		/// <summary>
		/// Convert a given string to a <see cref="ValidatorMode"/>.
		/// </summary>
		/// <param name="validatorMode">The string</param>
		/// <returns>The result <see cref="ValidatorMode"/>.</returns>
		/// <exception cref="ValidatorConfigurationException">when the string don't have a valid value.</exception>
		public static ValidatorMode ValidatorModeConvertFrom(string validatorMode)
		{
			if (string.IsNullOrEmpty(validatorMode))
				return ValidatorMode.UseAttribute;

			string vm = validatorMode.ToLowerInvariant();
			switch (vm)
			{
				case "useattribute":
					return ValidatorMode.UseAttribute;

				case "usexml":
					return ValidatorMode.UseXml;

				case "overrideattributewithxml":
					return ValidatorMode.OverrideAttributeWithXml;

				case "overridexmlwithattribute":
					return ValidatorMode.OverrideXmlWithAttribute;

				default:
					throw new ValidatorConfigurationException("Unexpected ValidatorMode :" + validatorMode);
			}
		}
	}
}
