using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class MaxValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			MaxValidator v = new MaxValidator();
			v.Initialize(new MaxAttribute());
			Assert.IsTrue(v.IsValid(0));
			v.Initialize(new MaxAttribute(1000));
			Assert.IsTrue(v.IsValid(3));
			Assert.IsTrue(v.IsValid((decimal)200.0));
			Assert.IsTrue(v.IsValid(1000));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("5"));
			Assert.IsFalse(v.IsValid(1001));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
			Assert.IsFalse(v.IsValid(long.MaxValue));
		}
	}
}
