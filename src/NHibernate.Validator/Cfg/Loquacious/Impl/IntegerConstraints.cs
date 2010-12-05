using System;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class IntegerConstraints<T> : BaseConstraints<IIntegerConstraints<T>>, IIntegerConstraints<T>
	{
		public IntegerConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IIntegerConstraints

		public IRuleArgsOptions Digits(int maxIntegerDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(maxIntegerDigits));
		}

		public IRuleArgsOptions Digits(int maxIntegerDigits, int maxFractionalDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(maxIntegerDigits, maxFractionalDigits));
		}

		public IRuleArgsOptions LessThanOrEqualTo(long maxValue)
		{
			return AddWithFinalRuleArgOptions(new MaxAttribute(maxValue));
		}

		public IRuleArgsOptions GreaterThanOrEqualTo(long minValue)
		{
			return AddWithFinalRuleArgOptions(new MinAttribute(minValue));
		}

		public IRuleArgsOptions IncludedBetween(long minValue, long maxValue)
		{
			return AddWithFinalRuleArgOptions(new RangeAttribute(minValue, maxValue));
		}

		public IRuleArgsOptions Whitih(long minValue, long maxValue)
		{
			return AddWithFinalRuleArgOptions(new WithinAttribute(minValue, maxValue));
		}

		public IRuleArgsOptions IsEAN()
		{
			return AddWithFinalRuleArgOptions(new EANAttribute());
		}

		#endregion

		#region Implementation of ISatisfier<T,IIntegerConstraints<T>>

		public IChainableConstraint<IIntegerConstraints<T>> Satisfy(Func<T, IConstraintValidatorContext, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		public IChainableConstraint<IIntegerConstraints<T>> Satisfy(Func<T, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedSimpleConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		#endregion
	}
}