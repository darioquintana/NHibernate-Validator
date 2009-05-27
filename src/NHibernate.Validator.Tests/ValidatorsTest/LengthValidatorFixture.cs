using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class LengthValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			LengthValidator v = new LengthValidator();
			v.Initialize(new LengthAttribute(5));
			Assert.IsTrue(v.IsValid("12", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("12345", null));
			Assert.IsFalse(v.IsValid("123456", null));
			Assert.IsFalse(v.IsValid(11, null));

			v.Initialize(new LengthAttribute(3, 6));
			Assert.IsTrue(v.IsValid("123", null));
			Assert.IsTrue(v.IsValid("123456", null));
			Assert.IsFalse(v.IsValid("12", null));
			Assert.IsFalse(v.IsValid("1234567", null));
		}
	}
}
