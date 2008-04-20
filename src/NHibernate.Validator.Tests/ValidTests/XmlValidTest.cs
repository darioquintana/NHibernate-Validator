using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ValidTests
{
	public class XmlValidTest : ValidTest
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}