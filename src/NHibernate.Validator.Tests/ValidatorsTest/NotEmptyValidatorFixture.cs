using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class NotEmptyValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			NotEmptyValidator v = new NotEmptyValidator();
			Assert.IsTrue(v.IsValid("abc", null));
			Assert.IsTrue(v.IsValid(new int[] { 1 }, null));
			Assert.IsTrue(v.IsValid(new List<int>(new int[] { 1 }), null));
			Assert.IsTrue(v.IsValid(null, null));

			Assert.IsFalse(v.IsValid("", null));
			Assert.IsFalse(v.IsValid("    ", null));
			Assert.IsFalse(v.IsValid(123, null));
			Assert.IsFalse(v.IsValid(new int[0], null));
			Assert.IsFalse(v.IsValid(new List<int>(), null));
		}
	}
}
