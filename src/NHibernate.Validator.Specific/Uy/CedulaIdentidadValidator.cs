using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Uy
{
	/// <summary>
	/// Validator Cedula identidad
	/// </summary>
	public class CedulaIdentidadValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value)
		{
			if (value == null || value.ToString() == string.Empty)
			{
				return true;
			}

			string cedula = value.ToString();
			if (cedula.Length > 8)
			{
				return false;
			}

			if (!IsInteger(cedula))
			{
				return false;
			}

			int[] dvs = new int[] { 2, 9, 8, 7, 6, 3, 4 };
			
			int sum = 0;

			for (int i = 0; i < cedula.Length - 1; i++)
			{
				int digito = Convert.ToInt32(cedula[i].ToString()) * dvs[i];
				sum += digito % 10;
			}

			return cedula[cedula.Length-1].ToString() == Convert.ToString((10 - sum % 10) % 10);
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