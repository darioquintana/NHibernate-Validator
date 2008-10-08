using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public abstract class AbstractLuhnValidator
	{
		private const string pattern = @"\d*$";
		private static readonly Regex regex = new Regex(pattern, RegexOptions.Compiled);
		public abstract int Multiplicator { get; }

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			var creditCard = value as string;
			if (string.IsNullOrEmpty(creditCard) || creditCard.Length > 19 || !regex.IsMatch(creditCard)
			    || ulong.Parse(creditCard) == 0)
			{
				return false;
			}

			IList<int> ints = new List<int>();
			foreach (char c in creditCard)
			{
				if (Char.IsDigit(c))
				{
					ints.Add(c - '0');
				}
			}

			int sum = 0;
			bool even = false;

			for (int index = ints.Count - 1; index >= 0; index--)
			{
				int digit = ints[index];
				if (even)
				{
					digit *= Multiplicator;
					if (digit > 9)
					{
						digit = digit / 10 + digit % 10;
					}
				}

				sum += digit;
				even = !even;
			}

			return sum % 10 == 0;
		}
	}
}