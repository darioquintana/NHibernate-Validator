using System;
using NHibernate.Validator.Constraints;
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
			Assert.IsTrue(v.IsValid(200.0m));
			Assert.IsFalse(v.IsValid(99));
			Assert.IsFalse(v.IsValid(1001));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
			Assert.IsFalse(v.IsValid(long.MaxValue));
		}

		[Test]
		public void Attribute()
		{
			RangeAttribute r = new RangeAttribute();
			Assert.AreEqual(long.MinValue, r.Min);
			Assert.AreEqual(long.MaxValue, r.Max);
			r = new RangeAttribute(100, 1000, "---");
			Assert.AreEqual(100, r.Min);
			Assert.AreEqual(1000, r.Max);
			Assert.AreEqual("---", r.Message);
		}

		private enum AEnum
		{
			A = 100
		}
		[Flags]
		private enum CarOptions
		{
			Spoiler = 0x02,
			FogLights = 0x04,
		}

		[Test]
		public void Extreme()
		{
			RangeValidator v = new RangeValidator();
			v.Initialize(new RangeAttribute(long.MinValue, 10000));
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

			v.Initialize(new RangeAttribute(-10000, long.MaxValue));
			Assert.IsTrue(v.IsValid(-10000));
			Assert.IsTrue(v.IsValid(-10000L));
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
			Assert.IsTrue(v.IsValid(-9999.99999f));
			Assert.IsTrue(v.IsValid(-9999.9999999999999999999999999d));

			Assert.IsFalse(v.IsValid(decimal.MinValue));
			Assert.IsFalse(v.IsValid(decimal.MinValue.ToString()));
			Assert.IsFalse(v.IsValid(double.MinValue));
			Assert.IsFalse(v.IsValid(double.MinValue + "9"));
		}
	}
}
