using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.ValidatorsTest;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	public class XmlValidatorsFixture : ValidatorsFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}