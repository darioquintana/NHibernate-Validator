using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class DigitsValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new DigitsAttribute(3);
			Assert.IsTrue(v.IsValid(0, null));
			Assert.IsTrue(v.IsValid(9, null));
			Assert.IsTrue(v.IsValid(99, null));
			Assert.IsTrue(v.IsValid(99.0, null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid("22", null));
			Assert.IsTrue(v.IsValid(103, null));
			Assert.IsTrue(v.IsValid(01, null));

			Assert.IsFalse(v.IsValid(1000, null));
			Assert.IsFalse(v.IsValid(10.1, null));
			Assert.IsFalse(v.IsValid(new object(), null));
			Assert.IsFalse(v.IsValid("aa.bb", null));

			v= new DigitsAttribute(3, 2);
			Assert.IsTrue(v.IsValid(0, null));
			Assert.IsTrue(v.IsValid(1, null));
			Assert.IsTrue(v.IsValid(100.100, null));
			Assert.IsTrue(v.IsValid(99.99, null));

			Assert.IsFalse(v.IsValid(1000.0, null));
			Assert.IsFalse(v.IsValid(9.233, null));
			Assert.IsFalse(v.IsValid("1233", null));
			

			v= new DigitsAttribute(0, 2);
			Assert.IsTrue(v.IsValid(0, null));
			Assert.IsTrue(v.IsValid(0.12, null));
			Assert.IsTrue(v.IsValid(0.1, null));
			Assert.IsTrue(v.IsValid(0.00000000000, null));

			Assert.IsFalse(v.IsValid(1.12, null));
			Assert.IsFalse(v.IsValid(0.123, null));
			
			
		}
	}
}
