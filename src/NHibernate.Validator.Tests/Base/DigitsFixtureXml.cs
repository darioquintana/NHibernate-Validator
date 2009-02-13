using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class DigitsFixtureXml : DigitsFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}