using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator
{
	public abstract class AbstractLuhnValidator
	{
		public abstract int multiplicator();

		public bool IsValid(object value)
		{
			if (value == null) return true;

			if (!(value is string)) return false;

			string creditCard = (string)value;

			char[] chars = creditCard.ToCharArray();

			IList<int> ints = new List<int>();
			foreach (char c in chars)
			{
				if (Char.IsDigit(c))
					ints.Add(c - '0');
			}

			int length = ints.Count;
			int sum = 0;
			bool even = false;

			for (int index = length - 1; index >= 0; index--)
			{
				int digit = ints[index];
				if (even)
				{
					digit *= multiplicator();
				}

				if (digit > 9)
				{
					digit = digit / 10 + digit % 10;
				}

				sum += digit;
				even = !even;
			}

			return sum % 10 == 0;
		}
	}
}
