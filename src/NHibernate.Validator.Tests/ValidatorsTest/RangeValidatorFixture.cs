using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class RangeValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			RangeValidator v = new RangeValidator();
			v.Initialize(new RangeAttribute());
			Assert.IsTrue(v.IsValid(long.MinValue));
			v.Initialize(new RangeAttribute(100, 1000));
			Assert.IsTrue(v.IsValid(100));
			Assert.IsTrue(v.IsValid(1000));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("105"));
			Assert.IsTrue(v.IsValid(200));
			Assert.IsTrue(v.IsValid((decimal)200.0));
			Assert.IsFalse(v.IsValid(99));
			Assert.IsFalse(v.IsValid(1001));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
			Assert.IsFalse(v.IsValid(long.MaxValue));
		}
	}
}
