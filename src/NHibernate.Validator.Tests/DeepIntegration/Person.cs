using System.Collections;
using System.Collections.Generic;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	public class Person
	{
		private ICollection<Person> children;
		private ICollection friends;
		private int id;
		private string name;
		private Person parent;

		protected Person() {}

		public Person(string name)
		{
			this.name = name;
		}

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		[NotEmpty, NotNull, Length(Min=2)]
		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		[Valid, Size(Max=10)]
		public virtual ICollection<Person> Children
		{
			get { return children; }
			set { children = value; }
		}

		[Valid]
		public virtual ICollection Friends
		{
			get { return friends; }
			set { friends = value; }
		}

		[Valid]
		public virtual Person Parent
		{
			get { return parent; }
			set { parent = value; }
		}
	}
}