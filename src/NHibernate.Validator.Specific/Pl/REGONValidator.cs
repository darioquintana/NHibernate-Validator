using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	public class REGONValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			if (value == null)
				return true;

			string regon = value.ToString();

			if (regon == string.Empty)
				return true;

			if (!HasValidFormat(regon))
				return false;

			return HasValidChecksum(regon);
		}

		#endregion

		private bool HasValidFormat(string regon)
		{
			var check = new Regex(@"^\d{9}$|^\d{14}$", RegexOptions.Compiled);

			return check.IsMatch(regon);
		}

		private bool HasValidChecksum(string number)
		{
			int[][] weights =
				{
					new[] {8, 9, 2, 3, 4, 5, 6, 7},
					new[] {2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8}
				};

			int[] selectedWeights = (number.Length == 9) ? weights[0] : weights[1];
			int totNumbers = number.Length;
			int result = 0;

			for (int i = 0; i < totNumbers - 1; ++i)
			{
				result += Int32.Parse(number[i].ToString())*selectedWeights[i];
			}

			result %= 11;

			return (result%10 == Int32.Parse(number[totNumbers - 1].ToString()));
		}
	}
}