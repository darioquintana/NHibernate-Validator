using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;

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

			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);

			p.PostalCode = "1234";
			InvalidValue[] iv = personValidator.GetInvalidValues(p);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Nieprawidlowy kod pocztowy.", iv[0].Message);

			p.PostalCode = "12-345";
			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);
		}
	}
}