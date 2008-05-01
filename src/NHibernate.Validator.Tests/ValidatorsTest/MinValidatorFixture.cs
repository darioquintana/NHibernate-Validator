using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class MinValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			MinValidator v = new MinValidator();
			v.Initialize(new MinAttribute());
			Assert.IsTrue(v.IsValid(0));
			v.Initialize(new MinAttribute(2));
			Assert.IsTrue(v.IsValid(3));
			Assert.IsTrue(v.IsValid(2));
			Assert.IsTrue(v.IsValid((decimal)200.0));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("5"));
			Assert.IsTrue(v.IsValid((long)int.MaxValue + 1));
			Assert.IsFalse(v.IsValid(1));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
		}
	}
}
