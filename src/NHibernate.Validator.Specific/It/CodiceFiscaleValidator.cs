using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.It
{
	public class CodiceFiscaleValidator : IValidator
	{

		public bool IsValid(object value,IConstraintValidatorContext constraintContext)
		{
			if (value == null) return true;
			String codiceFiscale = value.ToString();
			if (string.IsNullOrEmpty(codiceFiscale)) return true;
			codiceFiscale = codiceFiscale.Trim().ToUpper();
			// il codice fiscale DEVE essere composto da 16 caratteri.
			if (codiceFiscale.Length != 16) return false;

			// per il calcolo del check digit e la conversione in numero
			const string listaControllo = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			int[] listaCodiciPari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
			int[] listaCodiciDispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };

			int somma = 0;
			char[] caratteriChar = codiceFiscale.ToCharArray();
			for (int i = 0; i < 15; i++)
			{
				char c = caratteriChar[i];
				int x = "0123456789".IndexOf(c);
				if (x != -1)
					c = listaControllo[x];

				x = listaControllo.IndexOf(c);
				// Verifico che sia pari. Partendo da 0 il controllo è "scombinato"
				if ((i % 2) == 0)
					x = listaCodiciDispari[x];
				else
					x = listaCodiciPari[x];
				somma += x;
			}

			return (listaControllo[somma % 26] == codiceFiscale[15]);
		}
	}
}