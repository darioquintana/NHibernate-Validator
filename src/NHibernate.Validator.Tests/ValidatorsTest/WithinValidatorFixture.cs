using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class WithinValidatorFixture
	{
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
		public void Attribute()
		{
			var r = new WithinAttribute();
			Assert.AreEqual(double.MinValue, r.Min);
			Assert.AreEqual(double.MaxValue, r.Max);
			r = new WithinAttribute(100, 1000, "---");
			Assert.AreEqual(100, r.Min);
			Assert.AreEqual(1000, r.Max);
			Assert.AreEqual("---", r.Message);
		}

		[Test]
		public void Extreme()
		{
			var v = new WithinValidator();
			v.Initialize(new WithinAttribute(double.MinValue, 10000.9999999999D));
			Assert.IsTrue(v.IsValid(10000, null));
			Assert.IsTrue(v.IsValid(10000L, null));
			Assert.IsTrue(v.IsValid(123UL, null));
			Assert.IsTrue(v.IsValid(123U, null));
			Assert.IsTrue(v.IsValid((ushort) 5, null));
			Assert.IsTrue(v.IsValid((short) 5, null));
			Assert.IsTrue(v.IsValid(true, null));
			Assert.IsTrue(v.IsValid((byte) 100, null));
			Assert.IsTrue(v.IsValid((sbyte) 100, null));
			Assert.IsTrue(v.IsValid(AEnum.A, null));
			Assert.IsTrue(v.IsValid(CarOptions.Spoiler | CarOptions.FogLights, null));
			Assert.IsTrue(v.IsValid('A', null));
			Assert.IsTrue(v.IsValid(10000.98f, null));
			Assert.IsTrue(v.IsValid(10000.9999999998d, null));
			Assert.IsTrue(v.IsValid(decimal.MinValue, null));

			Assert.IsFalse(v.IsValid(10000.9999999999D, null));
			Assert.IsFalse(v.IsValid(decimal.MaxValue, null));
			Assert.IsFalse(v.IsValid(decimal.MaxValue.ToString(), null));
			Assert.IsFalse(v.IsValid(double.MaxValue, null));
			Assert.IsFalse(v.IsValid("1" + double.MaxValue, null));

			v.Initialize(new WithinAttribute(-10000.9999999999d, double.MaxValue));
			Assert.IsTrue(v.IsValid(123UL, null));
			Assert.IsTrue(v.IsValid(123U, null));
			Assert.IsTrue(v.IsValid((ushort) 5, null));
			Assert.IsTrue(v.IsValid((short) 5, null));
			Assert.IsTrue(v.IsValid(true, null));
			Assert.IsTrue(v.IsValid((byte) 100, null));
			Assert.IsTrue(v.IsValid((sbyte) 100, null));
			Assert.IsTrue(v.IsValid(AEnum.A, null));
			Assert.IsTrue(v.IsValid(CarOptions.Spoiler | CarOptions.FogLights, null));
			Assert.IsTrue(v.IsValid('A', null));
			Assert.IsTrue(v.IsValid(-9999.99f, null));
			Assert.IsTrue(v.IsValid(-10000.9999999998d, null));

			Assert.IsFalse(v.IsValid(-10000.9999999999D, null));
			Assert.IsFalse(v.IsValid(decimal.MinValue, null));
			Assert.IsFalse(v.IsValid(decimal.MinValue.ToString(), null));
			Assert.IsFalse(v.IsValid(double.MinValue, null));
			Assert.IsFalse(v.IsValid(double.MinValue + "9", null));
		}

		[Test]
		public void IsValid()
		{
			var v = new WithinValidator();
			v.Initialize(new WithinAttribute());
			Assert.IsTrue(v.IsValid(long.MinValue, null));
			v.Initialize(new WithinAttribute(100, 1000));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("105", null));
			Assert.IsTrue(v.IsValid(200, null));
			Assert.IsTrue(v.IsValid(200.0m, null));
			Assert.IsFalse(v.IsValid(100, null));
			Assert.IsFalse(v.IsValid(1000, null));
			Assert.IsFalse(v.IsValid(99, null));
			Assert.IsFalse(v.IsValid(1001, null));
			Assert.IsFalse(v.IsValid("aaa", null));
			Assert.IsFalse(v.IsValid(new object(), null));
			Assert.IsFalse(v.IsValid(long.MaxValue, null));
		}
	}
}