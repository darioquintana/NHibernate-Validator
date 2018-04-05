using System;
using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class PastValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new PastAttribute();
			Assert.That(v.IsValid(DateTime.Now.AddMilliseconds(-1), null), Is.True, "One ms in past");
			Assert.That(v.IsValid(new DateTime?(), null), Is.True, "null nullable date");
			Assert.That(v.IsValid(new DateTime?(DateTime.Now.AddDays(-1)), null), Is.True, "One day in past as nullable");
			Assert.That(v.IsValid(null, null), Is.True, "null");
			Assert.That(v.IsValid(new DateTime(), null), Is.True, "Min date");
			// Below test relies on execution to be fast enough for checking the attribute before the next tick...
			// So that is a flaky test, forget it.
			//Assert.That(v.IsValid(DateTime.Now, null), Is.False, "Now");
			Assert.That(v.IsValid(DateTime.Now.AddMilliseconds(+1), null), Is.False, "One ms in future");
			Assert.That(v.IsValid(DateTime.Now.ToString(), null), Is.False, "string");
			Assert.That(v.IsValid(123456, null), Is.False, "int");
		}
	}
}
