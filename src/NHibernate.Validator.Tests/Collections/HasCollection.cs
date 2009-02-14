using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System.Collections.Generic;

	public class HasCollection
	{
		[Size(Min = 2, Max = 5)]
		public IList<string> StringCollection;
	}

	public class HasCollectionDef: ValidationDef<HasCollection>
	{
		public HasCollectionDef()
		{
			Define(x => x.StringCollection).SizeBetween(2, 5);
		}
	}
}