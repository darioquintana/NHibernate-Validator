using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.It;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.It
{
	[TestFixture]
	public class CodFiscaleFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			CodiceFiscaleValidator v = new CodiceFiscaleValidator();

			/*
			 * Mi son basato sul servizio offerto da 
			 * http://www.comuni.it/servizi/codfisc/
			 * per calcolare codici fiscali formalmente validi su persone inventate
			 */

			//Syntax incorrect
			Assert.IsFalse(v.IsValid("BRTLSS4506L840X"));
			Assert.IsFalse(v.IsValid("BRT LSS 450 6L84 0"));
			Assert.IsFalse(v.IsValid("BRLSS4506L840"));

			//True value tests:
			Assert.IsTrue(v.IsValid("BRTLSS45H06L840X"));
			Assert.IsTrue(v.IsValid("BVLMNN85T51G693U"));
			Assert.IsTrue(v.IsValid("DVLPLS13L44H703Q"));
			Assert.IsTrue(v.IsValid("RGNGPL67C04A944U"));
			Assert.IsTrue(v.IsValid("CTNNCN67C04F839D"));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(string.Empty));
			
			//False value tests (on true value)
			Assert.IsFalse(v.IsValid("BRTLSS45H06L840Y"));
			Assert.IsFalse(v.IsValid("BVLMNN85T51G693M"));
			Assert.IsFalse(v.IsValid("DVLPLS13L44H704Q"));
			Assert.IsFalse(v.IsValid("RGNGOL67C04A944U"));
			Assert.IsFalse(v.IsValid("CTNNCN77C04F839D"));
			
		}

		[Test]
		public void ValidatorAttribute()
		{
			ClassValidator userValidator = GetClassValidator(typeof(Cliente));
			Cliente c = new Cliente();
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
			c.CodiceFiscale = "BRTLSS45H06L840Y";
			InvalidValue[] iv = userValidator.GetInvalidValues(c);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Codica fiscale FORMALMENTE incorretto.", iv[0].Message);
			c.CodiceFiscale = "BRTLSS45H06L840X";
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
			
		}

		public class Cliente
		{
			[CodiceFiscale]
			private string codFiscaleField = "BRTLSS45H06L840X";
			private string codFiscale;

			[CodiceFiscale]
			public string CodiceFiscale
			{
				get { return codFiscale; }
				set { codFiscale = value; }
			}
		}
	}
}