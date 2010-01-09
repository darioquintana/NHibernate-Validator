using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV85
{
    public class Parent
    {
        protected Parent()
        {
        }

        public Parent(string name)
        {
            Children = new List<Child>();
            Name = name;
        }

        public virtual long Id { get; set; }
        [Valid]
        public virtual IList<Child> Children { get; set; }

        public virtual string Name { get;  set; }
    }

    public class Child
    {
        protected Child()
        {
        }

        public Child(string name)
        {
            Name = name;
        }

        public virtual long Id { get; set; }
        public virtual Parent Parent { get; set; }

        [Length(Min = 3)]
        public virtual string Name { get; set; }
    }
}
