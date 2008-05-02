using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class DigitsValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			DigitsValidator v = new DigitsValidator();
			v.Initialize(new DigitsAttribute(3));
			Assert.IsTrue(v.IsValid(9));
			Assert.IsTrue(v.IsValid(99));
			Assert.IsTrue(v.IsValid(99.0));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid("22"));
			Assert.IsTrue(v.IsValid(103));
			Assert.IsTrue(v.IsValid(01));

			Assert.IsFalse(v.IsValid(1000));
			Assert.IsFalse(v.IsValid(10.1));
			Assert.IsFalse(v.IsValid(new object()));
			Assert.IsFalse(v.IsValid("aa.bb"));

			v.Initialize(new DigitsAttribute(3, 2));
			Assert.IsTrue(v.IsValid(100.100));
			Assert.IsTrue(v.IsValid(99.99));
			Assert.IsFalse(v.IsValid(1000.0));
			Assert.IsFalse(v.IsValid(9.233));
			Assert.IsFalse(v.IsValid("1233"));

			v.Initialize(new DigitsAttribute(0, 2));
			Assert.IsTrue(v.IsValid(0.12));
			Assert.IsFalse(v.IsValid(1.12));
			Assert.IsFalse(v.IsValid(0.123));
		}
	}
}
