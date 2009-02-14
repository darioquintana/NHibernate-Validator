using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System;

	public class Presenter
	{
		[NotNull]
		public String name;
	}

	public class PresenterDef : ValidationDef<Presenter>
	{
		public PresenterDef()
		{
			Define(x => x.name).NotNullable();
		}
	}
}