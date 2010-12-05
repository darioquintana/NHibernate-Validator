using System.Collections.Generic;
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
			var v = new SizeAttribute();
			Assert.IsTrue(v.IsValid(new int[0], null));

			v = new SizeAttribute(1, 3);
			Assert.IsTrue(v.IsValid(new int[1], null));
			Assert.IsTrue(v.IsValid(new int[3], null));
			Assert.IsTrue(v.IsValid(null, null));

			Assert.IsFalse(v.IsValid(new int[0], null));
			Assert.IsFalse(v.IsValid(new int[4], null));
			// Assert.IsFalse(v.IsValid("465", null)); <= if size can validate a string then is not a big problem
			Assert.IsFalse(v.IsValid(123456, null));
		}

		[Test]
		public void IsValidWithGenericCollections()
		{
			var v = new SizeAttribute();
			Assert.IsTrue(v.IsValid(new HashSet<int>(), null));

			v = new SizeAttribute(1, 3);
			Assert.IsTrue(v.IsValid(new HashSet<int> { 1 }, null));
			Assert.IsTrue(v.IsValid(new HashSet<int> { 1, 2, 3 }, null));

			Assert.IsFalse(v.IsValid(new HashSet<int> { 1, 2, 3, 4, 5, 6 }, null));
		}
	}
}
