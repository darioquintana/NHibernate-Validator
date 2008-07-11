using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.It
{
	/// <summary>
	/// Validator Partita IVA
	/// </summary>
	public class PartitaIvaValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			string piva = value.ToString().Trim().PadLeft(11, '0');
			if (piva.Length > 11)
			{
				return false;
			}

			if (!IsInteger(piva))
			{
				return false;
			}

			if (
				!((int.Parse(piva.Substring(0, 7)) != 0) && (int.Parse(piva.Substring(7, 3)) >= 0)
				  && (int.Parse(piva.Substring(7, 3)) < 201)))
			{
				return false;
			}

			int somma = 0;
			for (int i = 0; i < 10; i++)
			{
				int j = int.Parse(piva.Substring(i, 1));

				if ((i + 1) % 2 == 0)
				{
					j *= 2;
					char[] c = j.ToString("00").ToCharArray();
					somma += int.Parse(c[0].ToString());
					somma += int.Parse(c[1].ToString());
				}
				else
				{
					somma += j;
				}
			}

			if ((somma.ToString("00").Substring(1, 1) == "0") && (piva.Substring(10, 1) != "0"))
			{
				return false;
			}

			somma = int.Parse(piva.Substring(10, 1)) + int.Parse(somma.ToString("00").Substring(1, 1));

			return somma.ToString("00").Substring(1, 1) == "0";
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