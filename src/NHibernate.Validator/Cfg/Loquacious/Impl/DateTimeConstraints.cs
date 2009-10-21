using System;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class DateTimeConstraints<T> : BaseConstraints<IDateTimeConstraints<T>>, IDateTimeConstraints<T>
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

		#region Implementation of ISatisfier<T,IDateTimeConstraints<T>>

		public IChainableConstraint<IDateTimeConstraints<T>> Satisfy(Func<T, IConstraintValidatorContext, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		public IChainableConstraint<IDateTimeConstraints<T>> Satisfy(Func<T, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedSimpleConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		#endregion
	}
}