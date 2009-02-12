using System;
using System.Reflection;
using System.Text.RegularExpressions;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class StringConstraints : BaseConstraints, IStringConstraints
	{
		public StringConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IStringConstraints

		public IStringConstraintsChain NotNullable()
		{
			return AddWithStringConstraintsChain(new NotNullAttribute());
		}

		public IStringConstraintsChain NotEmpty()
		{
			return AddWithStringConstraintsChain(new NotEmptyAttribute());
		}

		public IStringConstraintsChain MaxLength(int maxLength)
		{
			return AddWithStringConstraintsChain(new LengthAttribute(maxLength));
		}

		public IStringConstraintsChain NotNullableAndNotEmpty()
		{
			return AddWithStringConstraintsChain(new NotNullNotEmptyAttribute());
		}

		public IStringConstraintsChain LengthBetween(int minLength, int maxLength)
		{
			return AddWithStringConstraintsChain(new LengthAttribute(minLength, maxLength));
		}

		public IStringConstraintsChain MatchWith(string regex)
		{
			return AddWithStringConstraintsChain(new PatternAttribute(regex));
		}

		public IStringConstraintsChain MatchWith(string regex, RegexOptions flags)
		{
			return AddWithStringConstraintsChain(new PatternAttribute(regex, flags));
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

		#endregion

		public IStringConstraintsChain AddWithStringConstraintsChain<TRuleArg>(TRuleArg ruleArgs)
			where TRuleArg : Attribute, IRuleArgs
		{
			AddRuleArg(ruleArgs);
			return new StringConstraintsChain(this, ruleArgs);
		}
	}

	public class StringConstraintsChain : IStringConstraintsChain
	{
		private readonly IStringConstraints parent;
		private readonly IRuleArgs constraintAttribute;

		public StringConstraintsChain(IStringConstraints parent, IRuleArgs constraintAttribute)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			this.parent = parent;
			this.constraintAttribute = constraintAttribute;
		}

		#region Implementation of IStringConstraintsChaining

		public IStringConstraints And
		{
			get { return parent; }
		}

		public IStringConstraintsChain WithMessage(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			constraintAttribute.Message = message;
			return this;
		}

		#endregion
	}
}