using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.IPFixture
{
	public class XmlIPAddressFixture : IPAddressFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}