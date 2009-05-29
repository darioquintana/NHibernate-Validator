namespace NHibernate.Validator.Tests.Specifics.NHV25
{
	public class XmlDictionaryValueKeyFixture : DictionaryValueKeyFixture
	{
		public override Validator.Engine.IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}
	}
}
