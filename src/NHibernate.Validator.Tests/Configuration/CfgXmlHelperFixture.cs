using NHibernate.Validator.Cfg;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class CfgXmlHelperFixture
	{
		[Test]
		public void ValidatorModeTest()
		{
			Assert.AreEqual(ValidatorMode.UseAttribute, CfgXmlHelper.ValidatorModeConvertFrom("UseAttribute"));
			Assert.AreEqual(ValidatorMode.UseXml, CfgXmlHelper.ValidatorModeConvertFrom("useXml"));
			Assert.AreEqual(ValidatorMode.OverrideXmlWithAttribute, CfgXmlHelper.ValidatorModeConvertFrom("OVERRIDEXmlWithAttribute"));
			Assert.AreEqual(ValidatorMode.OverrideAttributeWithXml, CfgXmlHelper.ValidatorModeConvertFrom("OvverrideAttributeWithXml"));
			Assert.AreEqual(ValidatorMode.UseAttribute, CfgXmlHelper.ValidatorModeConvertFrom(null));
			Assert.AreEqual(ValidatorMode.UseAttribute, CfgXmlHelper.ValidatorModeConvertFrom(string.Empty));
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void InvalidValidatorMode()
		{
			CfgXmlHelper.ValidatorModeConvertFrom("Not supported");
		}
	}
}
