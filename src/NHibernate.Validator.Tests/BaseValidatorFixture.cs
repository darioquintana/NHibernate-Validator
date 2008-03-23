using System;
using System.Collections.Generic;
using System.Text;
using log4net.Config;

namespace NHibernate.Validator.Tests
{
    public abstract class BaseValidatorFixture
    {
        static BaseValidatorFixture()
        {
            XmlConfigurator.Configure();
        }

        public virtual ClassValidator GetClassValidator(System.Type type)
        {
            return new ClassValidator(type);
        }
    }
}
