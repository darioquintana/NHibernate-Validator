using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class FloatConstraints : BaseConstraints<IFloatConstraints>, IFloatConstraints
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

		public IRuleArgsOptions IncludedBetween(long minValue, long maxValue)
		{
			return AddWithFinalRuleArgOptions(new RangeAttribute(minValue, maxValue));
		}

		#endregion
	}
}