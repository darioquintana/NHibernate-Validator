using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class FutureValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			FutureValidator v = new FutureValidator();
			Assert.IsTrue(v.IsValid(DateTime.Now.AddDays(+1), null));
			Assert.IsTrue(v.IsValid(new DateTime?(), null));
			Assert.IsTrue(v.IsValid(new DateTime?(DateTime.Now.AddDays(+1)), null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsFalse(v.IsValid(DateTime.Now, null));
			Assert.IsFalse(v.IsValid(new DateTime(), null));
			Assert.IsFalse(v.IsValid(DateTime.Now.ToString(), null));
			Assert.IsFalse(v.IsValid(123456, null));
		}
	}
}