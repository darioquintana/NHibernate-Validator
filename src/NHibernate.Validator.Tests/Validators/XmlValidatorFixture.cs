using System.Globalization;
using System.Reflection;
using System.Resources;

namespace NHibernate.Validator.Tests.Validators
{
	public class XmlValidatorsFixture : ValidatorsFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return
				new ClassValidator(type,
													 new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
																							 Assembly.GetExecutingAssembly()), new CultureInfo("en"),
													 ValidatorMode.UseXml);
		}
	}
}
