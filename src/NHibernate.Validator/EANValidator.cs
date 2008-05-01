using System;
using System.Collections.Generic;
using NHibernate.Validator.Engine;
using System.Globalization;

namespace NHibernate.Validator
{
	/// <summary>
	/// Validate EAN13 and UPC-A
	/// http://en.wikipedia.org/wiki/European_Article_Number
	/// </summary>
	public class EANValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			long val;
			string ean = value.ToString();
			if (ean.Length != 13 || !long.TryParse(ean, out val))
			{
				return false;
			}
			char[] chars = val.ToString(NumberFormatInfo.InvariantInfo).ToCharArray();

			IList<int> ints = new List<int>();
			foreach (char c in chars)
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
	}
}