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
			var args = new AssertTrueAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsFalse()
		{
			var args = new AssertFalseAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		#endregion
	}
}