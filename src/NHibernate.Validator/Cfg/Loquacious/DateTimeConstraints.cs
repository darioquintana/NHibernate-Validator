using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class DateTimeConstraints : BaseConstraints, IDateTimeConstraints
	{
		public DateTimeConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IDateTimeConstraints

		public IRuleArgsOptions IsInThePast()
		{
			return AddWithFinalRuleArgOptions(new PastAttribute());
		}

		public IRuleArgsOptions IsInTheFuture()
		{
			return AddWithFinalRuleArgOptions(new FutureAttribute());
		}

		#endregion
	}
}