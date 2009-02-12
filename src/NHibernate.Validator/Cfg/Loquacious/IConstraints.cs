using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IConstraints
	{
		void AddRuleArg(Attribute ruleArgs);
		IRuleArgsOptions AddWithFinalRuleArgOptions<TRuleArg>(TRuleArg ruleArgs) where TRuleArg : Attribute, IRuleArgs;
	}
}