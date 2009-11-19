using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class NotNullNotEmptyValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new NotNullNotEmptyAttribute();
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

		[Test]
		public void WhenEnumeratorIsDisposable_ShouldDispose()
		{
			var v = new NotNullNotEmptyAttribute();
			DisposableEnumerator.DisposedTimes = 0;
			v.IsValid(new DisposableEnumerable(), null);
			DisposableEnumerator.DisposedTimes.Should().Be.GreaterThan(0);
		}
	}
}
