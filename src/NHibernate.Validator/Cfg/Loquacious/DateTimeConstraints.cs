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
			var args = new PastAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsInTheFuture()
		{
			var args = new FutureAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		#endregion
	}
}