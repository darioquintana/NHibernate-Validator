using System.Net;
using NHibernate.Validator.Constraints;
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
			Assert.IsTrue(v.IsValid("192.168.0.1", null));
			Assert.IsTrue(v.IsValid("255.255.255.255", null));
			Assert.IsTrue(v.IsValid("192.168.0.0", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(IPAddress.Parse("143.24.20.36"), null));
			Assert.IsFalse(v.IsValid("", null));
			Assert.IsFalse(v.IsValid("aaa.bbb.ccc", null));
			Assert.IsFalse(v.IsValid("260.255.255.255", null));
			Assert.IsFalse(v.IsValid("192.999.0.0", null));
		}
	}
}
