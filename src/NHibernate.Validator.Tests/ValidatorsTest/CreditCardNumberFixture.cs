using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class CreditCardNumberFixture
	{
		[Test]
		public void IsValid()
		{
			AssertValid("541234567890125");
			AssertValid("4408041234567893");
			AssertValid("4417123456789113");
			AssertValid(null);
			
			AssertInvalid("");
			AssertInvalid("0");
			AssertInvalid("000000000000000");
			AssertInvalid("1234567890123456");
			AssertInvalid("4417123456789112");
			AssertInvalid("4408041234567890");
			AssertInvalid(5); // check any values different of string
		}

		public void AssertValid(object value)
		{
			var context = new ConstraintContextMock();
			CreditCardNumberValidator v = new CreditCardNumberValidator();
			Assert.IsTrue(v.IsValid(value,context));
		}

		public void AssertInvalid(object value)
		{
			var context = new ConstraintContextMock();
			CreditCardNumberValidator v = new CreditCardNumberValidator();
			Assert.IsFalse(v.IsValid(value, context));
		}
	}
}