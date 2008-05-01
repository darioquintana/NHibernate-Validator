using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class EANValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null) return true;

			if (!(value is string)) return false;

			string creditCard = (string)value;
			char[] chars = creditCard.ToCharArray();

			IList<int> ints = new List<int>();
			foreach (char c in chars)
			{
				if (Char.IsDigit(c)) ints.Add(c - '0');
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

	}
}
