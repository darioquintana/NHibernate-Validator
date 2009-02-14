using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System.Collections.Generic;

	public class HasShowCollection
	{
		[Valid, Size(Min = 2,Max = 3)]
		public IList<Show> Shows;
	}

	public class HasShowCollectionDef: ValidationDef<HasShowCollection>
	{
		public HasShowCollectionDef()
		{
			Define(x => x.Shows).HasValidElements().And.SizeBetween(2, 3);
		}
	}
}