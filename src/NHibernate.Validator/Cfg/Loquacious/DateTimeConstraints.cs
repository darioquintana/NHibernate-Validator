using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class DateTimeConstraints : BaseConstraints, IDateTimeConstraints
	{
		public DateTimeConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IDateTimeConstraints

		public void IsInThePast()
		{
			AddRuleArg(new PastAttribute());
		}

		public void IsInTheFuture()
		{
			AddRuleArg(new FutureAttribute());
		}

		#endregion
	}
}