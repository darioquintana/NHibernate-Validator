using System;
using System.Reflection;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class BaseConstraints : IConstraints
	{
		public BaseConstraints(IConstraintAggregator parent, MemberInfo member)
		{
			Parent = parent;
			Member = member;
		}

		protected MemberInfo Member { get; private set; }
		protected IConstraintAggregator Parent { get; private set; }

		#region IConstraints Members

		public void AddRuleArg(Attribute ruleArgs)
		{
			Parent.Add(Member, ruleArgs);
		}

		public IRuleArgsOptions AddWithFinalRuleArgOptions<TRuleArg>(TRuleArg ruleArgs) where TRuleArg : Attribute, IRuleArgs
		{
			AddRuleArg(ruleArgs);
			return new FinalRuleArgsOptions(ruleArgs);
		}

		#endregion
	}

	public class BaseConstraints<T> : BaseConstraints where T : class
	{
		public BaseConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		public IChainableConstraint<T> AddWithConstraintsChain<TRuleArg>(TRuleArg ruleArgs)
			where TRuleArg : Attribute, IRuleArgs
		{
			AddRuleArg(ruleArgs);
			return new ChainableConstraint<T>(this as T, ruleArgs);
		}
	}
}