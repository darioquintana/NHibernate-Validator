using System;
using System.Reflection;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class BaseConstraints
	{
		public BaseConstraints(IConstraintAggregator parent, MemberInfo member)
		{
			Parent = parent;
			Member = member;
		}

		protected MemberInfo Member { get; private set; }
		protected IConstraintAggregator Parent { get; private set; }

		public void AddRuleArg(Attribute ruleArgs)
		{
			Parent.Add(Member, ruleArgs);
		}
	}
}