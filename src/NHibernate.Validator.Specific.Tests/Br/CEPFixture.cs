using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Br;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.Br
{
	[TestFixture]
	public class CEPFixture : BaseValidatorFixture
	{
		public class Usuario
		{
			[CEP] private string cepField = "40280902";

			[CEP]
			public string Cep { get; set; }
		}

		[Test]
		public void IsValid()
		{
			var v = new CEPValidator();

			//Syntax incorrect
			Assert.IsFalse(v.IsValid("102602001", null));
			Assert.IsFalse(v.IsValid("102 60 200", null));
			Assert.IsFalse(v.IsValid("31.564973", null));
			Assert.IsFalse(v.IsValid("31564-973", null));

			//True value tests:
			Assert.IsTrue(v.IsValid("40280902", null));
			Assert.IsTrue(v.IsValid("40.280-902", null));
			Assert.IsTrue(v.IsValid("40750100", null));
			Assert.IsTrue(v.IsValid("40.750-100", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof (Usuario));
			var u = new Usuario();
			Assert.AreEqual(0, userValidator.GetInvalidValues(u).Length);
			u.Cep = "40280902x";
			InvalidValue[] iv = userValidator.GetInvalidValues(u);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Número de CEP inválido.", iv[0].Message);
			u.Cep = "40280902";
			Assert.AreEqual(0, userValidator.GetInvalidValues(u).Length);
		}
	}
}