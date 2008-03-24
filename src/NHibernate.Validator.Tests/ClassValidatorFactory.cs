using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace NHibernate.Validator.Tests
{
	public class ClassValidatorFactory
	{
		public static ClassValidator GetValidatorForUseXmlTest(System.Type type)
		{
			return new ClassValidator(type,
								   new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
								   Assembly.GetExecutingAssembly()),
								   new CultureInfo("en"),
								   ValidatorMode.UseXml);
		}
	}
}
