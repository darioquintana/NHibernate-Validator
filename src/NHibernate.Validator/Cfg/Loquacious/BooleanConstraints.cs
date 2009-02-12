using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class BooleanConstraints : BaseConstraints, IBooleanConstraints
	{
		public BooleanConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IBooleanConstraints

		public IRuleArgsOptions IsTrue()
		{
			return AddWithFinalRuleArgOptions(new AssertTrueAttribute());
		}

		public IRuleArgsOptions IsFalse()
		{
			return AddWithFinalRuleArgOptions(new AssertFalseAttribute());
		}

		#endregion
	}
}