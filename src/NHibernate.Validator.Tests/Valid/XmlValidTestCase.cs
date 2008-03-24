using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace NHibernate.Validator.Tests.Valid
{
    public class XmlValidTestCase : ValidTest
    {
        public override ClassValidator GetClassValidator(System.Type type)
        {
            return
                new ClassValidator(type,
                                   new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
                                   Assembly.GetExecutingAssembly()),
                                   new CultureInfo("en"),
                                   ValidatorMode.UseXml);
        }
    }
}
