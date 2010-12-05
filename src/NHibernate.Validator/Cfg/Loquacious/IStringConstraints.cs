using System.Text.RegularExpressions;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IStringConstraints: ISatisfier<string,IStringConstraints>
	{
		IChainableConstraint<IStringConstraints> NotNullable();
		IChainableConstraint<IStringConstraints> NotEmpty();
		IChainableConstraint<IStringConstraints> MaxLength(int maxLength);
		IChainableConstraint<IStringConstraints> MinLength(int minLength);
		IChainableConstraint<IStringConstraints> NotNullableAndNotEmpty();
		IChainableConstraint<IStringConstraints> LengthBetween(int minLength, int maxLength);
		IChainableConstraint<IStringConstraints> MatchWith(string regex);
		IChainableConstraint<IStringConstraints> MatchWith(string regex, RegexOptions flags);
		IRuleArgsOptions IsNumeric();
		IRuleArgsOptions IsEmail();
		IRuleArgsOptions IsIP();
		IRuleArgsOptions IsEAN();
		IRuleArgsOptions IsIBAN();
		IRuleArgsOptions IsCreditCardNumber();
		IRuleArgsOptions Digits(int maxIntegerDigits);
		IRuleArgsOptions Digits(int maxIntegerDigits, int maxFractionalDigits);
		IRuleArgsOptions FilePathExists();
	}
}