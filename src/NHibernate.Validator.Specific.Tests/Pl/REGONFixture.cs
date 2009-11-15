using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Specific.Tests.Pl
{
	/// <summary>
	/// Valid regon number consists of 9 or 14 digits.
	/// 
	/// Checksum algorithm based on documentation at
	/// http://wipos.p.lodz.pl/zylla/ut/nip-rego.html
	/// </summary>
	[TestFixture]
	public class REGONFixture : BaseValidatorFixture
	{
		public class Person
		{
			private string regon;
			[REGON] private string regonField = "590096454";

			[REGON]
			public string REGON
			{
				get { return regon; }
				set { regon = value; }
			}
		}

		[Test]
		public void IsValid()
		{
			var v = new REGONValidator();

			Assert.IsTrue(v.IsValid("590096454", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));

			Assert.IsFalse(v.IsValid("59009645", null));
			Assert.IsFalse(v.IsValid(" 590096454", null));
			Assert.IsFalse(v.IsValid("590096454 ", null));
			Assert.IsFalse(v.IsValid("590094654", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator personValidator = GetClassValidator(typeof (Person));
			var p = new Person();

			personValidator.GetInvalidValues(p).Should().Be.Empty();

			p.REGON = "590094654";
			var invalidValues = personValidator.GetInvalidValues(p);
			invalidValues.Should().Not.Be.Empty();
			invalidValues.Single().Message.Should().Be.EqualTo("Nieprawidlowy numer REGON.");

			p.REGON = "590096454";
			personValidator.GetInvalidValues(p).Should().Be.Empty();
		}
	}
}