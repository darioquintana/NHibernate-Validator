using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Pl;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.Pl
{
	/// <summary>
	/// Valid forms are: XXX-XXX-YY-YY or XX-XX-YYY-YYY.
	/// 
	/// Checksum algorithm based on documentation at
	/// http://wipos.p.lodz.pl/zylla/ut/nip-rego.html
	/// </summary>
	[TestFixture]
	public class NIPFixture : BaseValidatorFixture
	{
		public class Person
		{
			private string nip;
			[NIP] private string nipField = "77-92-125-257";

			[NIP]
			public string NIP
			{
				get { return nip; }
				set { nip = value; }
			}
		}

		[Test]
		public void IsValid()
		{
			var v = new NIPValidator();

			Assert.IsTrue(v.IsValid("77-92-125-257", null));
			Assert.IsTrue(v.IsValid("779-212-52-57", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));

			Assert.IsFalse(v.IsValid("77- 92-125-257", null));
			Assert.IsFalse(v.IsValid(" 77-92-125-257", null));
			Assert.IsFalse(v.IsValid("779-21-25-257 ", null));
			Assert.IsFalse(v.IsValid("7792125257", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator personValidator = GetClassValidator(typeof (Person));
			var p = new Person();

			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);

			p.NIP = "7792125257";
			InvalidValue[] iv = personValidator.GetInvalidValues(p);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Nieprawidlowy numer NIP.", iv[0].Message);

			p.NIP = "77-92-125-257";
			Assert.AreEqual(0, personValidator.GetInvalidValues(p).Length);
		}
	}
}