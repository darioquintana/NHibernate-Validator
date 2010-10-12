using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.IPFixture
{
	[TestFixture]
	public class IPAddressFixture : BaseValidatorFixture
	{
		[Test] //NHV-104
		public void DefaultMessageErrorShouldExists()
		{
			var computer = new Computer {IpAddress = "aaa.bbb.ccc"};
			IClassValidator classValidator = GetClassValidator(typeof(Computer));

			var invalidValue = classValidator.GetInvalidValues(computer).First();

			invalidValue.Message.Should().Not.Be.EqualTo("{validator.ipaddress}");
		}

		[Test]
		public void TestInvalidIPAddresses()
		{
			var computer = new Computer();
			IClassValidator classValidator = GetClassValidator(typeof (Computer));

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
			var computer = new Computer { IpAddress = "192.168.0.1" };
			IClassValidator classValidator = GetClassValidator(typeof (Computer));
			classValidator.GetInvalidValues(computer).Should().Be.Empty();

			computer.IpAddress = "255.255.255.255";
			classValidator.GetInvalidValues(computer).Should().Be.Empty();

			computer.IpAddress = "192.168.0.0";
			classValidator.GetInvalidValues(computer).Should().Be.Empty();
		}
	}
}