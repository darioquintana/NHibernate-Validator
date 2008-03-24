using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.Valid
{
    public class XmlValidTest : ValidTest
    {
        public override ClassValidator GetClassValidator(System.Type type)
        {
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
        }
    }
}
