using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Ar
{
	public class CUITValidator : IValidator
	{
		// http://es.wikipedia.org/wiki/C%C3%B3digo_%C3%9Anico_de_Identificaci%C3%B3n_Tributaria
		private static readonly Regex Check = new Regex(@"^(\d{11})|(\d{2}([ ]|[-])\d{8}([ ]|[-])\d)$", RegexOptions.Compiled);
		private static readonly Regex Separetors = new Regex(@"(\s+)|[-]", RegexOptions.Compiled);

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
			if (!Check.IsMatch(cuit))
			{
				return false;
			}
			cuit = Separetors.Replace(cuit, "");

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

			var verificationChar = (cuit.Length - (sum % 11));
			verificationChar = verificationChar == 11 ? 0 : verificationChar == 10 ? 9 : verificationChar;
			return verificationChar == char.GetNumericValue(cuit[cuit.Length - 1]);
		}
	}
}