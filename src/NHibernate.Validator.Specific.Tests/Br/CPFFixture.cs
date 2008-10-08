using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Br;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.Br
{
	[TestFixture]
	public class CPFFixture : BaseValidatorFixture
	{
		public class Usuario
		{
			[CPF] private string cpfField = "11111111111";

			[CPF]
			public string Cpf { get; set; }
		}

		[Test]
		public void IsValid()
		{
			var v = new CPFValidator();

			//Syntax incorrect
			Assert.IsFalse(v.IsValid("123.456.789-12"));
			Assert.IsFalse(v.IsValid("12345678912"));
			Assert.IsFalse(v.IsValid("12 4567 912"));
			Assert.IsFalse(v.IsValid("31.564973"));
			Assert.IsFalse(v.IsValid("3154-973"));

			//True value tests:
			Assert.IsTrue(v.IsValid("111.111.111-11"));
			Assert.IsTrue(v.IsValid("222.222.222-22"));
			Assert.IsTrue(v.IsValid("11111111111"));
			Assert.IsTrue(v.IsValid("22222222222"));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(string.Empty));
		}

		[Test]
		public void ValidatorAttribute()
		{
			ClassValidator userValidator = GetClassValidator(typeof (Usuario));
			var u = new Usuario();
			Assert.AreEqual(0, userValidator.GetInvalidValues(u).Length);
			u.Cpf = "111cx1111";
			InvalidValue[] iv = userValidator.GetInvalidValues(u);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Número de CPF inválido.", iv[0].Message);
			u.Cpf = "111.111.111-11";
			Assert.AreEqual(0, userValidator.GetInvalidValues(u).Length);
		}
	}
}