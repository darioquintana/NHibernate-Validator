using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class NotNullNotEmptyValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			NotNullNotEmptyValidator v = new NotNullNotEmptyValidator();
			var context = new ConstraintContextMock();
			Assert.IsTrue(v.IsValid("abc", context));
			Assert.IsTrue(v.IsValid(new int[] { 1 }, context));
			Assert.IsTrue(v.IsValid(new List<int>(new int[] { 1 }), context));

			Assert.IsFalse(v.IsValid(null, context));
			Assert.IsFalse(v.IsValid("", context));
			Assert.IsFalse(v.IsValid(123, context));
			Assert.IsFalse(v.IsValid(new int[0], context));
			Assert.IsFalse(v.IsValid(new List<int>(), context));
		}
	}
}
