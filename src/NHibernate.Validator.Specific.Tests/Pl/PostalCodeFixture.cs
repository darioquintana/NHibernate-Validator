using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Specific.Tests.Pl
{
	/// <summary>
	/// Valid code is XX-XXX, where X is a digit.
	/// </summary>
	[TestFixture]
	public class PostalCodeFixture : BaseValidatorFixture
	{
		public class Person
		{
			private string postalCode;
			[PostalCode] private string postalCodeField = "12-345";

			[PostalCode]
			public string PostalCode
			{
				get { return postalCode; }
				set { postalCode = value; }
			}
		}

		[Test]
		public void IsValid()
		{
			var v = new PostalCodeValidator();

			Assert.IsTrue(v.IsValid("12-345", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));

			Assert.IsFalse(v.IsValid("12 -345", null));
			Assert.IsFalse(v.IsValid("12345", null));
			Assert.IsFalse(v.IsValid(" 12-345", null));
			Assert.IsFalse(v.IsValid("12-345 ", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator personValidator = GetClassValidator(typeof (Person));
			var p = new Person();

			personValidator.GetInvalidValues(p).Should().Be.Empty();

			p.PostalCode = "1234";
			var iv = personValidator.GetInvalidValues(p);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("Nieprawidlowy kod pocztowy.");

			p.PostalCode = "12-345";
			personValidator.GetInvalidValues(p).Should().Be.Empty();
		}
	}
}