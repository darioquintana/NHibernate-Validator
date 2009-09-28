using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	public class PESELValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			if (value == null)
				return true;

			string pesel = value.ToString();

			if (pesel == string.Empty)
				return true;

			if (!HasValidFormat(pesel))
			{
				return false;
			}

			return HasValidChecksum(pesel);
		}

		#endregion

		private bool HasValidFormat(string pesel)
		{
			var check = new Regex(@"^\d{11}$", RegexOptions.Compiled);

			return check.IsMatch(pesel);
		}

		private bool HasValidChecksum(string number)
		{
			var multipleTable = new[] {1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1};
			int result = 0;

			for (int i = 0; i < number.Length; ++i)
			{
				result += Int32.Parse(number[i].ToString())*multipleTable[i];
			}

			return result%10 == 0;
		}
	}
}