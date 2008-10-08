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
			Assert.IsTrue(v.IsValid(DateTime.Now.AddDays(+1)));
			Assert.IsTrue(v.IsValid(new DateTime?()));
			Assert.IsTrue(v.IsValid(new DateTime?(DateTime.Now.AddDays(+1))));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsFalse(v.IsValid(DateTime.Now));
			Assert.IsFalse(v.IsValid(new DateTime()));
			Assert.IsFalse(v.IsValid(DateTime.Now.ToString()));
			Assert.IsFalse(v.IsValid(123456));
		}
	}
}