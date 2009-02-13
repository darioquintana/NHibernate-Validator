using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using NHibernate.Validator.Specific.Ar;

namespace NHibernate.Validator.Specific.Tests.Ar
{
	[TestFixture]
	public class CUITFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			CUITValidator v = new CUITValidator();

			//True value tests:
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(string.Empty));
			Assert.IsTrue(v.IsValid("20017391322"));
			Assert.IsTrue(v.IsValid("20-01739132-2"));
			Assert.IsTrue(v.IsValid("20 01739132 2"));

			//False value tests (on true value)
			Assert.IsFalse(v.IsValid("20 01739132/2"));
			Assert.IsFalse(v.IsValid("20-017391322"));
			Assert.IsFalse(v.IsValid("20017391323"));
			Assert.IsFalse(v.IsValid("A0017391322"));
			Assert.IsFalse(v.IsValid("2001739132"));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof(Cliente));
			Cliente c = new Cliente();
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
			c.Cuit = "AA";
			InvalidValue[] iv = userValidator.GetInvalidValues(c);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("CUIT no valido.", iv[0].Message);
			c.Cuit = "20017391322";
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
			c.CustomCuit = "AA";
			iv = userValidator.GetInvalidValues(c);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Not valid CUIT.", iv[0].Message);
		}

		public class Cliente
		{
			[CUIT]
			private string cuitField = "20-00108345-8";
			private string cuit;
			private string customCuit;

			[CUIT]
			public string Cuit
			{
				get { return cuit; }
				set { cuit = value; }
			}

			[CUIT(Message="Not valid CUIT.")]
			public string CustomCuit
			{
				get { return customCuit; }
				set { customCuit = value; }
			}
		}
	}
}