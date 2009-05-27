using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Ar
{
	public class CUITValidator : IValidator
	{
		private static readonly Regex check = new Regex(@"^(\d{11})|(\d{2}([ ]|[-])\d{8}([ ]|[-])\d)$", RegexOptions.Compiled);
		private static readonly Regex separetors = new Regex(@"(\s+)|[-]", RegexOptions.Compiled);

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}
			string cuit = value.ToString();
			if (string.Empty.Equals(cuit))
			{
				return true;
			}
			if (!check.IsMatch(cuit))
			{
				return false;
			}
			cuit = separetors.Replace(cuit, "");

			double sum = 0;
			bool bint = false;
			for (int i = 5, c = 0, j = 7; c != 10; i--, c++)
			{
				if (i >= 2)
				{
					sum += (char.GetNumericValue(cuit[c]) * i);
				}
				else
				{
					bint = true;
				}

				if (bint && j >= 2)
				{
					sum += (char.GetNumericValue(cuit[c]) * j);
					j--;
				}
			}

			return (cuit.Length - (sum % 11)) == char.GetNumericValue(cuit[cuit.Length - 1]);
		}
	}
}