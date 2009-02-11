using System.Text.RegularExpressions;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IStringConstraints
	{
		IStringConstraints NotNullable();
		IStringConstraints NotEmpty();
		IStringConstraints MaxLength(int maxLength);
		IStringConstraints NotNullableAndNotEmpty();
		IStringConstraints LengthBetween(int minLength, int maxLength);
		IStringConstraints MatchWith(string regex);
		IStringConstraints MatchWith(string regex, RegexOptions flags);
		void IsEmail();
		void IsIP();
		void IsEAN();
		void IsIBAN();
		void IsCreditCardNumber();
		void Digits(int integerDigits);
		void Digits(int integerDigits, int fractionalDigits);
	}
}