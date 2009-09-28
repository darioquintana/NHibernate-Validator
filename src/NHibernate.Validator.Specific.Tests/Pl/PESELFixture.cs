using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.Pl
{
	/// <summary>
	/// Check the following rules:
	/// - the length consist of 11 digits
	/// - has a valid checksum
	/// 
	/// Algorithm based on documentation at http://en.wikipedia.org/wiki/PESEL
	/// </summary>
	[TestFixture]
	public class PESELFixture : BaseValidatorFixture
	{
		public class Person
		{
			private string pesel;
			[PESEL] private string peselField = "49040501580";

			[PESEL]
			public string PESEL
			{
				get { return pesel; }
				set { pesel = value; }
			}
		}

		[Test]
		public void IsValid()
		{
			var v = new PESELValidator();

			Assert.IsTrue(v.IsValid("49040501580", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));

			Assert.IsFalse(v.IsValid("4904050158", null));
			Assert.IsFalse(v.IsValid(" 49040501580", null));
			Assert.IsFalse(v.IsValid("49040501580 ", null));
			Assert.IsFalse(v.IsValid("490 40501580", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator personValidator = GetClassValidator(typeof (Person));
			var p = new Person();

			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);

			p.PESEL = "4904050158";
			InvalidValue[] iv = personValidator.GetInvalidValues(p);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Nieprawidlowy numer PESEL.", iv[0].Message);

			p.PESEL = "49040501580";
			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);
		}
	}
}