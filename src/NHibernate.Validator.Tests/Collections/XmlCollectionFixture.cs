using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.Collections
{
	public class XmlCollectionFixture : CollectionFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}
