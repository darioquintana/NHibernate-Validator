using System.Reflection;
using System.Text.RegularExpressions;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class StringConstraints : BaseConstraints<IStringConstraints>, IStringConstraints
	{
		public StringConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IStringConstraints

		public IChainableConstraint<IStringConstraints> NotNullable()
		{
			return AddWithConstraintsChain(new NotNullAttribute());
		}

		public IChainableConstraint<IStringConstraints> NotEmpty()
		{
			return AddWithConstraintsChain(new NotEmptyAttribute());
		}

		public IChainableConstraint<IStringConstraints> MaxLength(int maxLength)
		{
			return AddWithConstraintsChain(new LengthAttribute(maxLength));
		}

		public IChainableConstraint<IStringConstraints> NotNullableAndNotEmpty()
		{
			return AddWithConstraintsChain(new NotNullNotEmptyAttribute());
		}

		public IChainableConstraint<IStringConstraints> LengthBetween(int minLength, int maxLength)
		{
			return AddWithConstraintsChain(new LengthAttribute(minLength, maxLength));
		}

		public IChainableConstraint<IStringConstraints> MatchWith(string regex)
		{
			return AddWithConstraintsChain(new PatternAttribute(regex));
		}

		public IChainableConstraint<IStringConstraints> MatchWith(string regex, RegexOptions flags)
		{
			return AddWithConstraintsChain(new PatternAttribute(regex, flags));
		}

		public IRuleArgsOptions IsEmail()
		{
			return AddWithFinalRuleArgOptions(new EmailAttribute());
		}

		public IRuleArgsOptions IsIP()
		{
			return AddWithFinalRuleArgOptions(new IPAddressAttribute());
		}

		public IRuleArgsOptions IsEAN()
		{
			return AddWithFinalRuleArgOptions(new EANAttribute());
		}

		public IRuleArgsOptions IsIBAN()
		{
			return AddWithFinalRuleArgOptions(new IBANAttribute());
		}

		public IRuleArgsOptions IsCreditCardNumber()
		{
			return AddWithFinalRuleArgOptions(new CreditCardNumberAttribute());
		}

		public IRuleArgsOptions Digits(int integerDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(integerDigits));
		}

		public IRuleArgsOptions Digits(int integerDigits, int fractionalDigits)
		{
			return AddWithFinalRuleArgOptions(new DigitsAttribute(integerDigits, fractionalDigits));
		}

		public IRuleArgsOptions FilePathExists()
		{
			return AddWithFinalRuleArgOptions(new FileExistsAttribute());
		}

		#endregion
	}
}