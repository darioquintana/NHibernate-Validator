using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Validate EAN13 and UPC-A
	/// http://en.wikipedia.org/wiki/European_Article_Number
	/// </summary>
	[Serializable]
	public class EANValidator : IValidator
	{
		private const string pattern = @"\d*$";
		private static readonly Regex regex = new Regex(pattern, RegexOptions.Compiled);

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			string ean = value.ToString();
			if (ean.Length != 13 || !regex.IsMatch(ean))
			{
				return false;
			}

			IList<int> ints = new List<int>();
			foreach (char c in ean)
			{
				if (Char.IsDigit(c))
				{
					ints.Add(c - '0');
				}
			}
			int length = ints.Count;
			int sum = 0;
			bool even = false;

			for (int index = length - 1; index >= 0; index--)
			{
				int digit = ints[index];
				if (even)
				{
					digit *= 3;
				}
				sum += digit;
				even = !even;
			}

			return sum % 10 == 0;
		}

		#endregion
	}
}