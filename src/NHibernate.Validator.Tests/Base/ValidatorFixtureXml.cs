using System.Globalization;
using System.Reflection;
using System.Resources;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class ValidatorFixtureXml : ValidatorFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}

		public override IClassValidator GetClassValidator(System.Type type, ResourceManager resource, CultureInfo culture)
		{
			return new ClassValidator(type,
			                          new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
			                                              Assembly.GetExecutingAssembly()), new CultureInfo("en"),
			                          ValidatorMode.UseExternal);
		}
	}
}