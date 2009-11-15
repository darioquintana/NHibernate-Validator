using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

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

			personValidator.GetInvalidValues(p).Should().Be.Empty();

			p.PESEL = "4904050158";
			var iv = personValidator.GetInvalidValues(p);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("Nieprawidlowy numer PESEL.");

			p.PESEL = "49040501580";
			personValidator.GetInvalidValues(p).Should().Be.Empty();
		}
	}
}