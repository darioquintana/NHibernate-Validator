using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Collections
{
	public class XmlCollectionFixture : CollectionFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}
