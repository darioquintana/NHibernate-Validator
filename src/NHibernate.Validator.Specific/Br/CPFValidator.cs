using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Br
{
	public class CPFValidator : IValidator
	{
		private static readonly Regex mask = new Regex(@"\d{3}\.\d{3}\.\d{3}\-\d{2}", RegexOptions.Compiled);

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			return ValidaCPF(value);
		}

		#endregion

		private static bool ValidaCPF(object value)
		{
			if (value == null)
			{
				return true;
			}

			string cpf = value.ToString();

			if (string.IsNullOrEmpty(cpf))
			{
				return true;
			}

			if (cpf.Length < 11)
			{
				return false;
			}

			if (cpf.Length == 11)
			{
				return Verifica(cpf);
			}

			if (mask.IsMatch(cpf))
			{
				return Verifica(cpf.Replace(".", "").Replace("-", ""));
			}

			return false;
		}

		private static bool Verifica(string cpf)
		{
			if (!IsInteger(cpf))
			{
				return false;
			}

			if (!VerificaDigito(cpf.Substring(0, 9), Convert.ToInt16(cpf[9].ToString())))
			{
				return false;
			}

			if (!VerificaDigito(cpf.Substring(0, 10), Convert.ToInt16(cpf[10].ToString())))
			{
				return false;
			}

			return true;
		}

		private static bool VerificaDigito(string cpf, int digito)
		{
			int soma = 0;
			int seq = cpf.Length + 1;
			int digitoCalculado;

			for (int i = 0; i < cpf.Length; i++)
			{
				int num = Convert.ToInt16(cpf[i].ToString());

				soma += num * seq;
				seq--;
			}

			int resto = soma % 11;

			if (resto < 2)
			{
				digitoCalculado = 0;
			}
			else
			{
				digitoCalculado = 11 - resto;
			}

			return digitoCalculado == digito;
		}

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