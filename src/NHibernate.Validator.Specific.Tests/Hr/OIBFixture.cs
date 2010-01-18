using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Hr;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Specific.Tests.Hr
{
	[TestFixture]
	public class OIBFixture : BaseValidatorFixture
	{
		public class User
		{
			[OIB]
			public string Oib { get; set; }
		}

		[Test]
		public void IsValid()
		{
			var oibValidator = new OIBValidator();

			// Correct Oib
			Assert.IsTrue(oibValidator.IsValid("84833009254", null));

			// Incorect Oib
			Assert.IsFalse(oibValidator.IsValid("84833009251", null));
			Assert.IsFalse(oibValidator.IsValid(string.Empty, null));
			Assert.IsFalse(oibValidator.IsValid(null, null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof (User));
			var user = new User();
			user.Oib = "84833009251";
			IEnumerable<InvalidValue> iv = userValidator.GetInvalidValues(user);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("OIB nije ispravan.");
			user.Oib = "84833009254";
			userValidator.GetInvalidValues(user).Should().Be.Empty();
		}
	}
}