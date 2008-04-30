using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class ClassValidatorFixture : BaseValidatorFixture
	{
		// this Fixture is to test some special case

		[Test]
		public void GetInvalidValuesOfBean()
		{
			ClassValidator cv = GetClassValidator(typeof(Address));
			Assert.AreEqual(0, cv.GetInvalidValues(null).Length);

			try
			{
				cv.GetInvalidValues(new Suricato());
				Assert.Fail("Accept an instance of another type");
			}
			catch(ArgumentException)
			{
				//ok
			}
		}

		[Test]
		public void GetInvalidValuesOfProperty()
		{
			ClassValidator cv = GetClassValidator(typeof(Address));
			Assert.AreEqual(0, cv.GetInvalidValues(null, "blacklistedZipCode").Length);

			try
			{
				cv.GetInvalidValues(new Suricato(), "blacklistedZipCode");
				Assert.Fail("Accept an instance of another type");
			}
			catch (ArgumentException)
			{
				//ok
			}
		}

	}
}
