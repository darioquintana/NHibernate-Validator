using System;
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
			Assert.IsTrue(v.IsValid(200.0m));
			Assert.IsTrue(v.IsValid(1000));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("5"));
			Assert.IsFalse(v.IsValid(1001));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
			Assert.IsFalse(v.IsValid(long.MaxValue));
		}

		private enum AEnum
		{
			A=100
		}
		[Flags]
		public enum CarOptions
		{
			Spoiler = 0x02,
			FogLights = 0x04,
		}

		[Test]
		public void Extreme()
		{
			MaxValidator v = new MaxValidator();
			v.Initialize(new MaxAttribute(10000));
			Assert.IsTrue(v.IsValid(10000));
			Assert.IsTrue(v.IsValid(10000L));
			Assert.IsTrue(v.IsValid(123UL));
			Assert.IsTrue(v.IsValid(123U));
			Assert.IsTrue(v.IsValid((ushort)5));
			Assert.IsTrue(v.IsValid((short)5));
			Assert.IsTrue(v.IsValid(true));
			Assert.IsTrue(v.IsValid((byte)100));
			Assert.IsTrue(v.IsValid((sbyte)100));
			Assert.IsTrue(v.IsValid(AEnum.A));
			Assert.IsTrue(v.IsValid(CarOptions.Spoiler | CarOptions.FogLights));
			Assert.IsTrue(v.IsValid('A'));
			Assert.IsTrue(v.IsValid(9999.99999f));
			Assert.IsTrue(v.IsValid(9999.9999999999999999999999999d));

			Assert.IsFalse(v.IsValid(decimal.MaxValue));
			Assert.IsFalse(v.IsValid(decimal.MaxValue.ToString()));
			Assert.IsFalse(v.IsValid(double.MaxValue));
			Assert.IsFalse(v.IsValid("1" + double.MaxValue));
		}
	}
}
