using System;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Checks wheter the member is a number having up to <c>IntegerDigits</c>,
	/// and <c>FractionalDigits</c> fractional digits.
	/// <remarks>
	/// <c>FractionalDigits</c> default value: 0.
	/// </remarks> 
	/// <code>
	/// For example:
	/// <example>
	/// - With [Digits(3)] //here FractionalDigits is zero.
	///		- Valid values: 0; 9; 99; 99.0; null; 01;
	///		- Invalid values: 1000; 10.1; "aa"; new object();
	/// 
	/// - With [Digits(2,3)]
	///		- Valid values: 0; 100.100; 99.99; 
	///		- Invalid values: 1000.0; 9.233; 1233; 
	/// 
	/// - With [Digits(0,2)]
	///		- Valid values: 0; 0.1; 0.12;
	///		- Invalid values: 1.12; 0.123
	/// </example>
	/// </code>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DigitsAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private string message = "{validator.digits}";
		public DigitsAttribute() {}

		public DigitsAttribute(int maxIntegerDigits)
		{
			IntegerDigits = maxIntegerDigits;
		}

		public DigitsAttribute(int maxIntegerDigits, int maxFractionalDigits) : this(maxIntegerDigits)
		{
			FractionalDigits = maxFractionalDigits;
		}

		public int IntegerDigits { get; set; }

		public int FractionalDigits { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		/// <summary>
		/// does the object/element pass the constraints
		/// </summary>
		/// <param name="value">Object to be validated</param>
		/// <param name="constraintContext"></param>
		/// <returns>if the instance is valid</returns>
		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			string stringValue;

			if (value is string)
			{
				try
				{
					stringValue = Convert.ToDouble(value).ToString();
				}
				catch (FormatException)
				{
					return false;
				}
			}
			else if (IsNumeric(value))
			{
				stringValue = value.ToString();
			}
			else
			{
				return false;
			}

			string separator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

			int pos = stringValue.IndexOf(separator);

			int left = (pos == -1) ? stringValue.Length : pos;
			int right = (pos == -1) ? 0 : stringValue.Length - pos - 1;

			if (left == 1 && stringValue[0] == '0')
			{
				left--;
			}

			return !(left > IntegerDigits || right > FractionalDigits);
		}

		#endregion

		private static bool IsNumeric(object expression)
		{
			double retNum;

			return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
		}
	}
}