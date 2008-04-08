using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.IPFixture
{
	public class XmlIPAddressFixture : IPAddressFixture
	{
		public override NHibernate.Validator.Engine.ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}
