using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.EmailTests
{
	public class XmlEmailFixture : EmailFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}