using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class SizeValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			SizeValidator v = new SizeValidator();
			
			v.Initialize(new SizeAttribute());
			Assert.IsTrue(v.IsValid(new int[0], null));
			
			v.Initialize(new SizeAttribute(1,3));
			Assert.IsTrue(v.IsValid(new int[1], null));
			Assert.IsTrue(v.IsValid(new int[3], null));
			Assert.IsTrue(v.IsValid(null, null));

			Assert.IsFalse(v.IsValid(new int[0], null));
			Assert.IsFalse(v.IsValid(new int[4], null));
			Assert.IsFalse(v.IsValid("465", null));
			Assert.IsFalse(v.IsValid(123456, null));
		}
	}
}
