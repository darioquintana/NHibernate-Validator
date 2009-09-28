using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	public class NIPValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			if (value == null)
				return true;

			string nip = value.ToString();

			if (nip == string.Empty)
			{
				return true;
			}

			if (!HasValidFormat(nip))
			{
				return false;
			}

			return HasValidChecksum(nip.Replace("-", ""));
		}

		#endregion

		private bool HasValidFormat(string nip)
		{
			var check = new Regex(@"^\d{3}-\d{3}-\d{2}-\d{2}$|^\d{2}-\d{2}-\d{3}-\d{3}$", RegexOptions.Compiled);

			return check.IsMatch(nip);
		}

		private bool HasValidChecksum(string number)
		{
			var multipleTable = new[] {6, 5, 7, 2, 3, 4, 5, 6, 7};
			int totNumbers = multipleTable.Length;
			int result = 0;

			for (int i = 0; i < totNumbers - 1; ++i)
			{
				result += Int32.Parse(number[i].ToString())*multipleTable[i];
			}

			result %= 11;

			return (result == Int32.Parse(number[totNumbers - 1].ToString()));
		}
	}
}