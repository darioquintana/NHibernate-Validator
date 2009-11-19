using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class LengthValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new LengthAttribute();
			Assert.IsTrue(v.IsValid("12", null));
			v = new LengthAttribute(5);
			Assert.IsTrue(v.IsValid("12", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("12345", null));
			Assert.IsFalse(v.IsValid("123456", null));
			Assert.IsFalse(v.IsValid(11, null));

			v= new LengthAttribute(3, 6);
			Assert.IsTrue(v.IsValid("123", null));
			Assert.IsTrue(v.IsValid("123456", null));
			Assert.IsFalse(v.IsValid("12", null));
			Assert.IsFalse(v.IsValid("1234567", null));
		}
	}
}
