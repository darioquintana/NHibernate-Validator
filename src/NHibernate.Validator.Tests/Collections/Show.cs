using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System;

	public class Show
	{
		[NotNull]
		public String name;

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

	public class ShowDef : ValidationDef<Show>
	{
		public ShowDef()
		{
			Define(x => x.name).NotNullable();
		}
	}
}