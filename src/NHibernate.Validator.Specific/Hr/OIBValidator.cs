using System;
using System.Linq;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Hr
{
	/// <summary>
	/// 
	/// </summary>
	public class OIBValidator : IValidator
	{
		#region IValidator Members

		/// <summary>
		/// does the object/element pass the constraints
		/// </summary>
		/// <param name="value">Object to be validated</param>
		/// <param name="constraintValidatorContext">Context for the validator constraint</param>
		/// <returns>if the instance is valid</returns>
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return value == null ? false : CheckOib(value.ToString());
		}

		#endregion

		/// <summary>
		/// Checks the Croatian Personal Identification Number (OIB - Osobni Identifikacijski Broj). 
		/// Algoritham is ISO7064 MOD 11,10
		/// </summary>
		/// <param name="oib">The oib.</param>
		/// <returns>True if OIB is valid else false.</returns>
		private static bool CheckOib(string oib)
		{
			if (oib.Length != 11)
			{
				return false;
			}

			long oibL;
			int[] digits;
			if (long.TryParse(oib, out oibL))
			{
				digits = Array.ConvertAll(oib.ToCharArray(), c => Int32.Parse(c.ToString()));
			}
			else
			{
				return false;
			}

			return digits[10]
			       == 11 - digits.Take(10).Aggregate(10, (x1, x2) => ((x1 + x2) % 10 == 0 ? 10 : (x1 + x2) % 10) * 2 % 11);
		}
	}
}