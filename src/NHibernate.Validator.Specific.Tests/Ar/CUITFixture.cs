using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using NHibernate.Validator.Specific.Ar;
using SharpTestsEx;

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
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));
			Assert.IsTrue(v.IsValid("20017391322", null));
			Assert.IsTrue(v.IsValid("20-01739132-2", null));
			Assert.IsTrue(v.IsValid("20 01739132 2", null));
			Assert.IsTrue(v.IsValid("20-13499715-0", null));

			//False value tests (on true value)
			Assert.IsFalse(v.IsValid("20 01739132/2", null));
			Assert.IsFalse(v.IsValid("20-017391322", null));
			Assert.IsFalse(v.IsValid("20017391323", null));
			Assert.IsFalse(v.IsValid("A0017391322", null));
			Assert.IsFalse(v.IsValid("2001739132", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof(Cliente));
			Cliente c = new Cliente();
			var iv = userValidator.GetInvalidValues(c);
			iv.Should().Be.Empty();
			c.Cuit = "AA";
			iv = userValidator.GetInvalidValues(c);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("CUIT no valido.");
			c.Cuit = "20017391322";
			userValidator.GetInvalidValues(c).Should().Be.Empty();
			c.CustomCuit = "AA";
			iv = userValidator.GetInvalidValues(c);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("Not valid CUIT.");
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