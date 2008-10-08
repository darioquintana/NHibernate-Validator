using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System;

	public class Presenter
	{
		[NotNull] public String name;
	}
}