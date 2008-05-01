using System;
using System.Collections.Generic;
using System.Text;
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
			Assert.IsTrue(v.IsValid("12"));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("12345"));
			Assert.IsFalse(v.IsValid("123456"));
			Assert.IsFalse(v.IsValid(11));

			v.Initialize(new LengthAttribute(3, 6));
			Assert.IsTrue(v.IsValid("123"));
			Assert.IsTrue(v.IsValid("123456"));
			Assert.IsFalse(v.IsValid("12"));
			Assert.IsFalse(v.IsValid("1234567"));
		}
	}
}
