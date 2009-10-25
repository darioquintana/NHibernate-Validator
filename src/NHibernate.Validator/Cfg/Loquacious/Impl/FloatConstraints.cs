using System;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class FloatConstraints<T> : BaseConstraints<IFloatConstraints<T>>, IFloatConstraints<T>
	{
		public FloatConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IFloatConstraints

		public IRuleArgsOptions Digits(int integerDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(integerDigits));
		}

		public IRuleArgsOptions Digits(int integerDigits, int fractionalDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(integerDigits, fractionalDigits));
		}

		public IRuleArgsOptions LessThanOrEqualTo(long maxValue)
		{
			return AddWithFinalRuleArgOptions(new MaxAttribute(maxValue));
		}

		public IRuleArgsOptions GreaterThanOrEqualTo(long minValue)
		{
			return AddWithFinalRuleArgOptions(new MinAttribute(minValue));
		}

		public IRuleArgsOptions LessThanOrEqualTo(decimal maxValue)
		{
			return AddWithFinalRuleArgOptions(new DecimalMaxAttribute(maxValue));
		}

		public IRuleArgsOptions GreaterThanOrEqualTo(decimal minValue)
		{
			return AddWithFinalRuleArgOptions(new DecimalMinAttribute(minValue));
		}

		public IRuleArgsOptions IncludedBetween(long minValue, long maxValue)
		{
			return AddWithFinalRuleArgOptions(new RangeAttribute(minValue, maxValue));
		}

		public IRuleArgsOptions Whitih(double minValue, double maxValue)
		{
			return AddWithFinalRuleArgOptions(new WithinAttribute(minValue, maxValue));
		}

		#endregion

		#region Implementation of ISatisfier<T,IFloatConstraints<T>>

		public IChainableConstraint<IFloatConstraints<T>> Satisfy(Func<T, IConstraintValidatorContext, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		public IChainableConstraint<IFloatConstraints<T>> Satisfy(Func<T, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedSimpleConstraint<T>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		#endregion
	}
}