using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class DecimalMaxValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			DecimalMaxValidator v = new DecimalMaxValidator();
			v.Initialize(new DecimalMaxAttribute());
			Assert.IsTrue(v.IsValid(0m, null));
			Assert.IsTrue(v.IsValid(0, null));
			Assert.IsTrue(v.IsValid(0.0, null));
			Assert.IsFalse(v.IsValid("a", null));
			Assert.IsTrue(v.IsValid("0", null)); 

			v.Initialize(new DecimalMaxAttribute(100.567m));
			Assert.IsTrue(v.IsValid(100.567m, null));
			Assert.IsTrue(v.IsValid(100.567m.ToString(), null));
			Assert.IsTrue(v.IsValid(100.567f, null));
			Assert.IsTrue(v.IsValid(100.567, null));

			Assert.IsFalse(v.IsValid(100.568m.ToString(), null));
			Assert.IsFalse(v.IsValid(100.568f, null));

			Assert.IsFalse(v.IsValid(decimal.MaxValue, null));
			Assert.IsFalse(v.IsValid(Int64.MaxValue, null));
			Assert.IsFalse(v.IsValid(long.MaxValue, null));
		}
	}
}
