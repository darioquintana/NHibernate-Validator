using System;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class EANValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			EANValidator v = new EANValidator();
			Assert.IsTrue(v.IsValid("9782266156066"));
			Assert.IsTrue(v.IsValid(9782266156066));
			Assert.IsTrue(v.IsValid(9782266156066U));
			Assert.IsTrue(v.IsValid(9782266156066D));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(new Ean13("97","822661","5606")));
			Assert.IsFalse(v.IsValid(""));
			Assert.IsFalse(v.IsValid("9782266156067"));
			Assert.IsFalse(v.IsValid("12345678901234"));
			Assert.IsFalse(v.IsValid(9782266156067));
		}

		// Is is a dirty implementation only for test scope
		public class Ean13
		{
			private string checksumDigit;
			private string countryCode = "00";
			private string manufacturerCode;
			private string productCode;

			public Ean13() { }

			public Ean13(string manufacturerCode, string productCode)
			{
				CountryCode = "00";
				ManufacturerCode = manufacturerCode;
				ProductCode = productCode;
				CalculateChecksumDigit();
			}

			public Ean13(string countryCode, string manufacturerCode, string productCode)
			{
				CountryCode = countryCode;
				ManufacturerCode = manufacturerCode;
				ProductCode = productCode;
				CalculateChecksumDigit();
			}

			public string CountryCode
			{
				get { return countryCode; }

				set
				{
					while (value.Length < 2)
					{
						value = "0" + value;
					}
					countryCode = value;
					CalculateChecksumDigit();
				}
			}

			public string ManufacturerCode
			{
				get { return manufacturerCode; }

				set
				{
					manufacturerCode = value;
					CalculateChecksumDigit();
				}
			}

			public string ProductCode
			{
				get { return productCode; }

				set
				{
					productCode = value;
					CalculateChecksumDigit();
				}
			}

			public string ChecksumDigit
			{
				get { return checksumDigit; }
			}

			public void CalculateChecksumDigit()
			{
				string temp = CountryCode + ManufacturerCode + ProductCode;
				int sum = 0;

				// Calculate the checksum digit here.
				for (int i = temp.Length; i >= 1; i--)
				{
					int digit = Convert.ToInt32(temp.Substring(i - 1, 1));
					if (i % 2 == 0)
					{
						// odd
						sum += digit * 3;
					}
					else
					{
						// even
						sum += digit * 1;
					}
				}

				int checkSum = (10 - (sum % 10)) % 10;
				checksumDigit = checkSum.ToString();
			}

			public override string ToString()
			{
				return CountryCode + ManufacturerCode + ProductCode + checksumDigit;
			}
		}

	}
}