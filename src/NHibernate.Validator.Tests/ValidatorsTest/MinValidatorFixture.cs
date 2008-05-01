using System;
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
			Assert.IsTrue(v.IsValid(-10000));
			Assert.IsTrue(v.IsValid(-10000L));
			Assert.IsTrue(v.IsValid(123UL));
			Assert.IsTrue(v.IsValid(123U));
			Assert.IsTrue(v.IsValid((ushort) 5));
			Assert.IsTrue(v.IsValid((short) 5));
			Assert.IsTrue(v.IsValid(true));
			Assert.IsTrue(v.IsValid((byte) 100));
			Assert.IsTrue(v.IsValid((sbyte) 100));
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

		[Test]
		public void IsValid()
		{
			MinValidator v = new MinValidator();
			v.Initialize(new MinAttribute());
			Assert.IsTrue(v.IsValid(0));
			v.Initialize(new MinAttribute(2));
			Assert.IsTrue(v.IsValid(3));
			Assert.IsTrue(v.IsValid(2));
			Assert.IsTrue(v.IsValid((decimal) 200.0));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("5"));
			Assert.IsTrue(v.IsValid((long) int.MaxValue + 1));
			Assert.IsFalse(v.IsValid(1));
			Assert.IsFalse(v.IsValid("aaa"));
			Assert.IsFalse(v.IsValid(new object()));
		}
	}
}