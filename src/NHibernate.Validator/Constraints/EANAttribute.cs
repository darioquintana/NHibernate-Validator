using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The Attribute element has to represent an EAN-13 or UPC-A
	/// which aims to check for user mistake, not actual number validity!
	/// http://en.wikipedia.org/wiki/European_Article_Number
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class EANAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private const string Pattern = @"\d*$";
		private static readonly Regex Regex = new Regex(Pattern, RegexOptions.Compiled);
		private string message = "{validator.ean}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			string ean = value.ToString();
			if (ean.Length != 13 || !Regex.IsMatch(ean))
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