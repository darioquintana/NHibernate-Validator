using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Collections
{
	public class XmlCollectionFixture : CollectionFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}
