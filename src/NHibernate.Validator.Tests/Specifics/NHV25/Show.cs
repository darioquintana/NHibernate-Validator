using System;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV25
{
	public class Show
	{
		[NotNull] public String name;

		public Show()
		{
		}

		public Show(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name ?? "null";
		}
	}
}