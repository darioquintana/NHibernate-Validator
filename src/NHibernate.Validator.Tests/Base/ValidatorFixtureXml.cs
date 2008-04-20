using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class ValidatorFixtureXml : ValidatorFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}