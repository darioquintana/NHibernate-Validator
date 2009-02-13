using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class ValidatorFixtureXml : ValidatorFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}