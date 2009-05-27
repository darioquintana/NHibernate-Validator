using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class MinValidatorFixture
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
		public void Extreme()
		{
			MinValidator v = new MinValidator();
			v.Initialize(new MinAttribute(-10000));
			Assert.IsTrue(v.IsValid(-10000, null));
			Assert.IsTrue(v.IsValid(-10000L, null));
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
			Assert.IsTrue(v.IsValid(-9999.99999f, null));
			Assert.IsTrue(v.IsValid(-9999.9999999999999999999999999d, null));

			Assert.IsFalse(v.IsValid(decimal.MinValue, null));
			Assert.IsFalse(v.IsValid(decimal.MinValue.ToString(), null));
			Assert.IsFalse(v.IsValid(double.MinValue, null));
			Assert.IsFalse(v.IsValid(double.MinValue + "9", null));
		}

		[Test]
		public void IsValid()
		{
			MinValidator v = new MinValidator();
			v.Initialize(new MinAttribute());
			Assert.IsTrue(v.IsValid(0, null));
			v.Initialize(new MinAttribute(2));
			Assert.IsTrue(v.IsValid(3, null));
			Assert.IsTrue(v.IsValid(2, null));
			Assert.IsTrue(v.IsValid((decimal) 200.0, null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("5", null));
			Assert.IsTrue(v.IsValid((long) int.MaxValue + 1, null));
			Assert.IsFalse(v.IsValid(1, null));
			Assert.IsFalse(v.IsValid("aaa", null));
			Assert.IsFalse(v.IsValid(new object(), null));
		}
	}
}