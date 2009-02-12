using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
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
			Assert.AreEqual(ValidatorMode.UseExternal, CfgXmlHelper.ValidatorModeConvertFrom("useExternal"));
			Assert.AreEqual(ValidatorMode.OverrideExternalWithAttribute, CfgXmlHelper.ValidatorModeConvertFrom("OVERRIDEExternalWithAttribute"));
			Assert.AreEqual(ValidatorMode.OverrideAttributeWithExternal, CfgXmlHelper.ValidatorModeConvertFrom("OverrideAttributeWithExternal"));
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
