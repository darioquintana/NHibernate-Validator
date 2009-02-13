using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class IntegerConstraints: BaseConstraints<IIntegerConstraints>, IIntegerConstraints
	{
		public IntegerConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IIntegerConstraints

		public IRuleArgsOptions Digits(int digits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(digits));
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

		public IRuleArgsOptions IsEAN()
		{
			return AddWithFinalRuleArgOptions(new EANAttribute());
		}

		#endregion
	}
}