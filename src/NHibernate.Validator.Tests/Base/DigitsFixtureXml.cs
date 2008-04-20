using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class DigitsFixtureXml : DigitsFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}