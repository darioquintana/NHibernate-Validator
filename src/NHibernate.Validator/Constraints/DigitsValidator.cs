using System;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DigitsValidator : IInitializableValidator<DigitsAttribute>
	{
		private int fractionalDigits;
		private int integerDigits;

		#region IInitializableValidator<DigitsAttribute> Members

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

			return !(left > integerDigits || right > fractionalDigits);
		}

		/// <summary>
		/// Take the annotations values and Initialize the Validator
		/// </summary>
		/// <param name="parameters">parameters</param>
		public void Initialize(DigitsAttribute parameters)
		{
			integerDigits = parameters.IntegerDigits;
			fractionalDigits = parameters.FractionalDigits;
		}

		#endregion

		private static bool IsNumeric(object expression)
		{
			double retNum;

			return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
		}
	}
}