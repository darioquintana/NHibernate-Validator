using System;
using System.Reflection;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IConstraintAggregator
	{
		void Add(MemberInfo member, Attribute ruleArgs);
	}
}