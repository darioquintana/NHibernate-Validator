using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/*
 * 2008-07-06
 * Information sources
 * http://www.europebanks.info/ibanguide.htm#6
 * http://www.bancastato.ch/lista_paesi_iban-2.pdf
 * http://community.visual-basic.it/lucianob/archive/2008/04/04/22451.aspx
 * http://www.swift.com/index.cfm?item_id=61334
*/

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class IBANValidator
	{
		private const int WorldMaxLength = 34;

		/// <summary>
		/// Definitions of countries IBAN (Key=Country code as defined in ISO 3166; Value=IbanDefinition)
		/// </summary>
		private static readonly Dictionary<string, Regex> defs = new Dictionary<string, Regex>(195);

		static IBANValidator()
		{
			RegistrDefinition("AD", @"^(AD)(\d{2})(\d{4})(\d{4})(\w{12})$");
			RegistrDefinition("AT", @"^(AT)(\d{2})(\d{5})(\d{11})$");
			RegistrDefinition("BE", @"^(BE)(\d{2})(\d{3})(\d{7})(\d{2})$");
			RegistrDefinition("BA", @"^(BA)(\d{2})(\d{3})(\d{3})(\d{8})(\d{2})$");
			RegistrDefinition("BG", @"^(BG)(\d{2})(\w{4})(\d{4})(?<nd3>\d{2})(\w{8})$");
			RegistrDefinition("HR", @"^(HR)(\d{2})(\d{7})(\d{10})$");
			RegistrDefinition("CY", @"^(CY)(\d{2})(\w{3})(\d{5})(\w{16})$");
			RegistrDefinition("CZ", @"^(CZ)(\d{2})(\d{4})(\d{6})(\d{10})$");
			RegistrDefinition("DK", @"^(DK)(\d{2})(\d{4})(\d{9})(\d{1})$");
			RegistrDefinition("EE", @"^(EE)(\d{2})(\d{2})(\d{2})(\d{11})(\d{1})$");
			RegistrDefinition("FI", @"^(FI)(\d{2})(\d{6})(\d{7})(\d{1})$");
			RegistrDefinition("FR", @"^(FR)(\d{2})(\d{5})(\d{5})(\w{11})(\d{2})$");
			RegistrDefinition("DE", @"^(DE)(\d{2})(\d{8})(\d{10})$");
			RegistrDefinition("GI", @"^(GI)(\d{2})([A-Z]{4})(\d{15})$");
			RegistrDefinition("GR", @"^(GR)(\d{2})(\d{3})(\d{4})(\w{16})$");
			RegistrDefinition("HU", @"^(HU)(\d{2})(\d{3})(\d{4})(\d{1})(\d{15})(\d{1})$");
			RegistrDefinition("IS", @"^(IS)(\d{2})(\d{4})(\d{2})(\d{6})(\d{10})$");
			RegistrDefinition("IE", @"^(IE)(\d{2})([A-Z]{4})(\d{6})(\d{8})$");
			RegistrDefinition("IL", @"^(IL)(\d{2})(\d{3})(\d{3})(\d{13})$");
			RegistrDefinition("IT", @"^(IT)(\d{2})([A-Z])(\d{5})(\d{5})(\w{12})$");
			RegistrDefinition("LV", @"^(LV)(\d{2})([A-Z]{4})(\w{13})$");
			RegistrDefinition("LI", @"^(LI)(\d{2})(\d{5})(\w{12})$");
			RegistrDefinition("LT", @"^(LT)(\d{2})(\d{5})(\d{11})$");
			RegistrDefinition("LU", @"^(LU)(\d{2})(\d{3})(\w{13})$");
			RegistrDefinition("MK", @"^(MK)(\d{2})(\d{3})(\w{10})(\d{2})$");
			RegistrDefinition("MT", @"^(MT)(\d{2})([A-Z]{4})(\d{5})(\w{18})$");
			RegistrDefinition("MU", @"^(MU)(\d{2})([A-Z]{4})(\d{2})(\d{2})(\d{12})(\d{3})([A-Z]{3})$");
			RegistrDefinition("MC", @"^(MC)(\d{2})(\d{5})(\d{5})(\w{11})(\d{2})$");
			RegistrDefinition("ME", @"^(ME)(\d{2})(\d{3})(\d{13})(\d{2})$");
			RegistrDefinition("NL", @"^(NL)(\d{2})([A-Z]{4})(\d{10})$");
			RegistrDefinition("NO", @"^(NO)(\d{2})(\d{4})(\d{6})(\d{1})$");
			RegistrDefinition("PL", @"^(PL)(\d{2})(\d{8})(\d{16})$");
			RegistrDefinition("PT", @"^(PT)(\d{2})(\d{4})(\d{4})(\d{11})(\d{2})$");
			RegistrDefinition("RO", @"^(RO)(\d{2})([A-Z]{4})(\w{16})$");
			RegistrDefinition("SM", @"^(SM)(\d{2})([A-Z])(\d{5})(\d{5})(\w{12})$");
			RegistrDefinition("RS", @"^(RS)(\d{2})(\d{3})(\d{13})(\d{2})$");
			RegistrDefinition("SK", @"^(SK)(\d{2})(\d{4})(\d{6})(\d{10})$");
			RegistrDefinition("SI", @"^(SI)(\d{2})(\d{5})(\d{8})(\d{2})$");
			RegistrDefinition("ES", @"^(ES)(\d{2})(\d{4})(\d{4})(\d{1})(\d{1})(\d{10})$");
			RegistrDefinition("SE", @"^(SE)(\d{2})(\d{3})(\d{16})(\d{1})$");
			RegistrDefinition("CH", @"^(CH)(\d{2})(\d{5})(\w{12})$");
			RegistrDefinition("TN", @"^(TN)(59)(\d{2})(\d{3})(\d{13})(\d{2})$");
			RegistrDefinition("TR", @"^(TR)(\d{2})(\d{5})(\w{1})(\w{16})$");
			RegistrDefinition("GB", @"^(GB)(\d{2})([A-Z]{4})(\d{6})(\d{8})$");
		}

		private static void RegistrDefinition(string countryCode, string pattern)
		{
			defs[countryCode] = new Regex(pattern, RegexOptions.Compiled);
		}

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			string strIBAN = value.ToString();
			if (string.IsNullOrEmpty(strIBAN))
			{
				return true;
			}

			string cleanIban;
			if (!GetValidatable(strIBAN, out cleanIban))
			{
				return false;
			}

			if (cleanIban.Length > WorldMaxLength || cleanIban.Length < 2)
			{
				return false;
			}

			string countryCode = cleanIban.Substring(0, 2);
			Regex syntax;
			defs.TryGetValue(countryCode, out syntax);

			if (syntax == null || !syntax.IsMatch(cleanIban))
			{
				return false;
			}

			return IsValidCin(cleanIban);
		}

		private static bool IsValidCin(string cleanIban)
		{
			//CIN verify: DE89370400440532013000

			//Step 1) append first 4 characters (country+cin) to the end of iban: 370400440532013000DE89
			string ibanToCheck = cleanIban.Substring(4) + cleanIban.Substring(0, 4);

			//Step 2) alfa characters encoding: 370400440532013000131489
			ibanToCheck = CharactersEncoding(ibanToCheck);

			//Step3) check mod of the CIN
			return IsValidModOfCin(ibanToCheck);
		}

		/// <summary>
		/// IBAN cleaner
		/// </summary>
		/// <param name="iban">Not cleaned IBAN</param>
		/// <param name="validable">The validatable IBAN</param>
		/// <returns>true if the <paramref name="iban"/> have all valid character; otherwise false</returns>
		private static bool GetValidatable(IEnumerable<char> iban, out string validable)
		{
			validable = null;
			const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			const string separators = " -";
			var sb = new StringBuilder(WorldMaxLength);
			foreach (char c in iban)
			{
				char cc = char.ToUpperInvariant(c);
				if (validCharacters.IndexOf(cc) >= 0)
				{
					sb.Append(cc);
				}
				else
				{
					if (separators.IndexOf(c) < 0)
					{
						return false;
					}
				}
			}
			validable = sb.ToString();
			return true;
		}

		private static string CharactersEncoding(IEnumerable<char> iban)
		{
			const string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var result = new StringBuilder(WorldMaxLength * 2);
			foreach (char c in iban)
			{
				if (alpha.IndexOf(c) < 0)
				{
					result.Append(c);
				}
				else
				{
					//encode alphabetical char: A=10 ... Z=35
					result.Append(alpha.IndexOf(c) + 10);
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Validate the CIN for a given encoded IBAN
		/// </summary>
		/// <param name="encodedIban">The encoded IBAN.</param>
		/// <returns>true if the CIN is valid (mod=1); otherwise false</returns>
		private static bool IsValidModOfCin(string encodedIban)
		{
			int r = 0;
			for (int x = 0; x < encodedIban.Length; x++)
			{
				r = ((r * 10) + int.Parse(encodedIban[x].ToString())) % 97;
			}

			return r == 1;
		}
	}
}