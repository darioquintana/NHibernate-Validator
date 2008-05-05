using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	public class WebImageFixtureXml : WebImageFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseXmlTest(type);
		}
	}
}
