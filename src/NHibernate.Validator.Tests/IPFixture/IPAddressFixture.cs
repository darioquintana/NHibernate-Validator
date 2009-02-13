using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.IPFixture
{
	[TestFixture]
	public class IPAddressFixture : BaseValidatorFixture
	{
		[Test]
		public void TestInvalidIPAddresses()
		{
			Computer computer = new Computer();
			computer.IpAddress = "aaa.bbb.ccc";
			IClassValidator classValidator = GetClassValidator(typeof(Computer));
			InvalidValue[] ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(1, ivs.Length, "aaa.bbb.ccc is not a valid IP");

			computer.IpAddress = "260.255.255.255";
			ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(1, ivs.Length, "260.255.255.255 is not a valid ip");

			computer.IpAddress = "192.999.0.0";
			ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(1, ivs.Length, "192.999.0.0 is not a valid ip");
		}

		[Test]
		public void TestValidIPAddresses()
		{
			Computer computer = new Computer();
			computer.IpAddress = "192.168.0.1";
			IClassValidator classValidator = GetClassValidator(typeof(Computer));
			InvalidValue[] ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(0, ivs.Length);

			computer.IpAddress = "255.255.255.255";
			ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(0, ivs.Length);

			computer.IpAddress = "192.168.0.0";
			ivs = classValidator.GetInvalidValues(computer);
			Assert.AreEqual(0, ivs.Length, "192.168.0.0 is a valid ip");
		}
	}
}
