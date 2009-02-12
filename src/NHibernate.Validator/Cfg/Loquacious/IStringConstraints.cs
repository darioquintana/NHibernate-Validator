using System.Text.RegularExpressions;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IStringConstraints
	{
		IStringConstraintsChain NotNullable();
		IStringConstraintsChain NotEmpty();
		IStringConstraintsChain MaxLength(int maxLength);
		IStringConstraintsChain NotNullableAndNotEmpty();
		IStringConstraintsChain LengthBetween(int minLength, int maxLength);
		IStringConstraintsChain MatchWith(string regex);
		IStringConstraintsChain MatchWith(string regex, RegexOptions flags);
		IRuleArgsOptions IsEmail();
		IRuleArgsOptions IsIP();
		IRuleArgsOptions IsEAN();
		IRuleArgsOptions IsIBAN();
		IRuleArgsOptions IsCreditCardNumber();
		IRuleArgsOptions Digits(int integerDigits);
		IRuleArgsOptions Digits(int integerDigits, int fractionalDigits);
	}

	public interface IStringConstraintsChain
	{
		IStringConstraints And { get; }
		IStringConstraintsChain WithMessage(string message);
	}
}