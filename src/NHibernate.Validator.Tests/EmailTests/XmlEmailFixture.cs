using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.EmailTests
{
	public class XmlEmailFixture : EmailFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}