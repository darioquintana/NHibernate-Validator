using System;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class DigitsValidator : IInitializableValidator<DigitsAttribute>
	{
		int integerDigits;
		int fractionalDigits;
		/// <summary>
		/// does the object/element pass the constraints
		/// </summary>
		/// <param name="value">Object to be validated</param>
		/// <returns>if the instance is valid</returns>
		public bool IsValid(object value)
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

		static bool IsNumeric(object Expression)
		{
			double retNum;

			return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
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
	}
}