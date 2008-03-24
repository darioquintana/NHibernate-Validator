using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests
{
	public class XmlEmailFixture : EmailFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}
