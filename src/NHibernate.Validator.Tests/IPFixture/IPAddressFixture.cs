using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.IPFixture
{
	[TestFixture]
	public class IPAddressFixture : BaseValidatorFixture
	{
		[Test]
		public void TestInvalidIPAddresses()
		{
			Computer computer = new Computer();
			IClassValidator classValidator = GetClassValidator(typeof(Computer));

			computer.IpAddress = "aaa.bbb.ccc";
			classValidator.GetInvalidValues(computer).Should("aaa.bbb.ccc is not a valid IP").Not.Be.Empty();

			computer.IpAddress = "260.255.255.255";
			classValidator.GetInvalidValues(computer).Should("260.255.255.255 is not a valid IP").Not.Be.Empty();

			computer.IpAddress = "192.999.0.0";
			classValidator.GetInvalidValues(computer).Should("192.999.0.0 is not a valid IP").Not.Be.Empty();
		}

		[Test]
		public void TestValidIPAddresses()
		{
			Computer computer = new Computer();
			computer.IpAddress = "192.168.0.1";
			IClassValidator classValidator = GetClassValidator(typeof(Computer));
			classValidator.GetInvalidValues(computer).Should().Be.Empty();

			computer.IpAddress = "255.255.255.255";
			classValidator.GetInvalidValues(computer).Should().Be.Empty();

			computer.IpAddress = "192.168.0.0";
			classValidator.GetInvalidValues(computer).Should().Be.Empty();
		}
	}
}
