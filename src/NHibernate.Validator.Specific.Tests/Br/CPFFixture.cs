using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Br;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

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
			Assert.IsFalse(v.IsValid("123.456.789-12", null));
			Assert.IsFalse(v.IsValid("12345678912", null));
			Assert.IsFalse(v.IsValid("12 4567 912", null));
			Assert.IsFalse(v.IsValid("31.564973", null));
			Assert.IsFalse(v.IsValid("3154-973", null));

			//True value tests:
			Assert.IsTrue(v.IsValid("111.111.111-11", null));
			Assert.IsTrue(v.IsValid("222.222.222-22", null));
			Assert.IsTrue(v.IsValid("11111111111", null));
			Assert.IsTrue(v.IsValid("22222222222", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof (Usuario));
			var u = new Usuario();
			userValidator.GetInvalidValues(u).Should().Be.Empty();
			u.Cpf = "111cx1111";
			var iv = userValidator.GetInvalidValues(u);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("Número de CPF inválido.");

			u.Cpf = "111.111.111-11";
			userValidator.GetInvalidValues(u).Should().Be.Empty();
		}
	}
}