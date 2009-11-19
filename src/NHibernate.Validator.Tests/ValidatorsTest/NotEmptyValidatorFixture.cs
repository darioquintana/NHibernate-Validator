using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class NotEmptyValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new NotEmptyAttribute();
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

		[Test]
		public void WhenEnumeratorIsDisposable_ShouldDispose()
		{
			var v = new NotEmptyAttribute();
			DisposableEnumerator.DisposedTimes = 0;
			v.IsValid(new DisposableEnumerable(), null);
			DisposableEnumerator.DisposedTimes.Should().Be.GreaterThan(0);
		}
	}

	public class DisposableEnumerable: IEnumerable
	{
		public IEnumerator GetEnumerator()
		{
			return new DisposableEnumerator();
		}
	}

	public class DisposableEnumerator: IEnumerator, IDisposable
	{
		[ThreadStatic]
		public static int DisposedTimes;

		public bool MoveNext()
		{
			return false;
		}

		public void Reset()
		{
		}

		public object Current
		{
			get { return null; }
		}

		public void Dispose()
		{
			DisposedTimes++;
		}
	}
}
