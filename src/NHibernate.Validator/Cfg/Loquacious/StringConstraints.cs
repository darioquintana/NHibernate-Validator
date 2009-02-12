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
			var args = new NotNullAttribute();
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain NotEmpty()
		{
			var args = new NotEmptyAttribute();
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain MaxLength(int maxLength)
		{
			var args = new LengthAttribute(maxLength);
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain NotNullableAndNotEmpty()
		{
			var args = new NotNullNotEmptyAttribute();
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain LengthBetween(int minLength, int maxLength)
		{
			var args = new LengthAttribute(minLength, maxLength);
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain MatchWith(string regex)
		{
			var args = new PatternAttribute(regex);
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IStringConstraintsChain MatchWith(string regex, RegexOptions flags)
		{
			var args = new PatternAttribute(regex, flags);
			AddRuleArg(args);
			return new StringConstraintsChain(this, args);
		}

		public IRuleArgsOptions IsEmail()
		{
			var args = new EmailAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsIP()
		{
			var args = new IPAddressAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsEAN()
		{
			var args = new EANAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsIBAN()
		{
			var args = new IBANAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions IsCreditCardNumber()
		{
			var args = new CreditCardNumberAttribute();
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions Digits(int integerDigits)
		{
			var args = new DigitsAttribute(integerDigits);
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		public IRuleArgsOptions Digits(int integerDigits, int fractionalDigits)
		{
			var args = new DigitsAttribute(integerDigits, fractionalDigits);
			AddRuleArg(args);
			return new FinalRuleArgsOptions(args);
		}

		#endregion
	}

	public class StringConstraintsChain: IStringConstraintsChain
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