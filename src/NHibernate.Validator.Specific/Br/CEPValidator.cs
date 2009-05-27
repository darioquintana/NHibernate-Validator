using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Br
{
	public class CEPValidator : IValidator
	{
		private static readonly Regex mask = new Regex(@"\d{2}\.\d{3}\-\d{3}", RegexOptions.Compiled);

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			string cep = value.ToString();

			if (string.Empty.Equals(cep))
			{
				return true;
			}

			if (cep.Length < 8)
			{
				return false;
			}

			if (cep.Length == 8)
			{
				if (!IsInteger(cep))
				{
					return false;
				}
			}

			if (cep.Length > 8)
			{
				if (!mask.IsMatch(cep))
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		private static bool IsInteger(string theValue)
		{
			long val;
			if (long.TryParse(theValue, out val))
			{
				return val > 0;
			}
			return false;
		}
	}
}