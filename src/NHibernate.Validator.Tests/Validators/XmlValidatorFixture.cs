using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Reflection;

namespace NHibernate.Validator.Tests.Validators
{
    public class XmlValidatorsFixture : ValidatorsFixture
    {
        public override ClassValidator GetClassValidator(System.Type type)
        {
            return new ClassValidator(type, new ResourceManager(
                                                                "NHibernate.Validator.Tests.Resource.Messages",
                                                                Assembly.GetExecutingAssembly()),
                                                               new CultureInfo("en"), "UseXml");
        } 
    }
}
