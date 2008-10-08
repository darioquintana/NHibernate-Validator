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
			Assert.IsTrue(v.IsValid("abc"));
			Assert.IsTrue(v.IsValid(new int[] { 1 }));
			Assert.IsTrue(v.IsValid(new List<int>(new int[] { 1 })));
			Assert.IsTrue(v.IsValid(null));

			Assert.IsFalse(v.IsValid(""));
			Assert.IsFalse(v.IsValid("    "));
			Assert.IsFalse(v.IsValid(123));
			Assert.IsFalse(v.IsValid(new int[0]));
			Assert.IsFalse(v.IsValid(new List<int>()));
		}
	}
}
