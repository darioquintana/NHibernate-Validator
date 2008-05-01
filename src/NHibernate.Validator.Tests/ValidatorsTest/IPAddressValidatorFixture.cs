using System.Net;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class IPAddressValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			IPAddressValidator v = new IPAddressValidator();
			Assert.IsTrue(v.IsValid("192.168.0.1"));
			Assert.IsTrue(v.IsValid("255.255.255.255"));
			Assert.IsTrue(v.IsValid("192.168.0.0"));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(IPAddress.Parse("143.24.20.36")));
			Assert.IsFalse(v.IsValid(""));
			Assert.IsFalse(v.IsValid("aaa.bbb.ccc"));
			Assert.IsFalse(v.IsValid("260.255.255.255"));
			Assert.IsFalse(v.IsValid("192.999.0.0"));
		}
	}
}
