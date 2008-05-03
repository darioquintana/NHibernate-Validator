using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class CreditCardNumberFixture
	{
		[Test]
		public void IsValid()
		{
			CreditCardNumberValidator v = new CreditCardNumberValidator();
			Assert.IsTrue(v.IsValid("541234567890125"));
			Assert.IsTrue(v.IsValid("4408041234567893"));
			Assert.IsTrue(v.IsValid("4417123456789113"));
			Assert.IsTrue(v.IsValid(null));

			Assert.IsFalse(v.IsValid(""));
			Assert.IsFalse(v.IsValid("0"));
			Assert.IsFalse(v.IsValid("000000000000000"));
			Assert.IsFalse(v.IsValid("1234567890123456"));
			Assert.IsFalse(v.IsValid("4417123456789112"));
			Assert.IsFalse(v.IsValid("4408041234567890"));
			Assert.IsFalse(v.IsValid(5)); // check any values different of string
		}
	}
}