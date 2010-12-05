using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class DecimalMinValidatorFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new DecimalMinAttribute();
			Assert.IsTrue(v.IsValid(0, null));
			Assert.IsTrue(v.IsValid(1, null));

			v= new DecimalMinAttribute(10.1456m);
			Assert.IsTrue(v.IsValid(10.1456m, null));
			Assert.IsTrue(v.IsValid(10.1457m, null));
			Assert.IsTrue(v.IsValid(10.1456m.ToString(), null));
			Assert.IsTrue(v.IsValid(10.145600000001f.ToString(), null));
			Assert.IsTrue(v.IsValid(10.1456f, null));
			Assert.IsTrue(v.IsValid(int.MaxValue, null));
			Assert.IsTrue(v.IsValid(long.MaxValue, null));
			Assert.IsTrue(v.IsValid(decimal.MaxValue, null));
			Assert.IsTrue(v.IsValid(Int64.MaxValue, null));



			Assert.IsFalse(v.IsValid(10.1455m, null));
			Assert.IsFalse(v.IsValid(10.1455m.ToString(), null));
			Assert.IsFalse(v.IsValid(10.1455f.ToString(), null));
			Assert.IsFalse(v.IsValid(10.1455f, null));
			Assert.IsFalse(v.IsValid(int.MinValue, null));
			Assert.IsFalse(v.IsValid(decimal.MinValue, null));
			Assert.IsFalse(v.IsValid(long.MinValue, null));
		}

		[Test]
		public void CanApplyDecimalMinAttribute()
		{
			var validator = GetClassValidator(typeof(FooDecimalMax));

			var testCase1 = new FooDecimalMax { DecimalValue = 1.01m };
			validator.GetInvalidValues(testCase1).Should().Be.Empty();

			var testCase2 = new FooDecimalMax { DecimalValue = -2.01m };
			validator.GetInvalidValues(testCase2).Should().Not.Be.Empty();
		}

		private class FooDecimalMax
		{
			[DecimalMin(-1.51)]
			public decimal DecimalValue { get; set; }
		}
	}
}
