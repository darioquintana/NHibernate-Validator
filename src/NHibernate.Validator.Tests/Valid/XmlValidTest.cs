using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

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
