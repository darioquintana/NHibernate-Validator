using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	public class HasArrayWithValid
	{
		[Valid,Size(Min = 2, Max = 4)]
		public Show[] Shows;
	}

	public class HasArrayWithValidDef: ValidationDef<HasArrayWithValid>
	{
		public HasArrayWithValidDef()
		{
			Define(x => x.Shows).HasValidElements().And.SizeBetween(2, 4);
		}
	}
}