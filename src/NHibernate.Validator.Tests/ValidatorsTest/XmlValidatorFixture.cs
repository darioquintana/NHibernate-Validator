using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.ValidatorsTest;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	public class XmlValidatorsFixture : ValidatorsFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}