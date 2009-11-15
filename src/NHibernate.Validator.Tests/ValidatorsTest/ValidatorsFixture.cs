using System;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	/// <summary>
	/// Fixture to validate the existing validators
	/// </summary>
	[TestFixture]
	public class ValidatorsFixture : BaseValidatorFixture
	{
		[Test]
		public void LengthTest()
		{
			IClassValidator validator = GetClassValidator(typeof(FooLength));

			FooLength f1 = new FooLength(1, "hola");
			validator.GetInvalidValues(f1).Should().Be.Empty();

			FooLength f2 = new FooLength(1, string.Empty);
			validator.GetInvalidValues(f2).Should().Not.Be.Empty();

			FooLength f3 = new FooLength(1, null);
			validator.GetInvalidValues(f3).Should().Not.Be.Empty();
		}

		[Test]
		public void NotEmptyTest()
		{
			IClassValidator validator = GetClassValidator(typeof(FooNotEmpty));

			FooNotEmpty f1 = new FooNotEmpty("hola");
			FooNotEmpty f2 = new FooNotEmpty(string.Empty);

			validator.AssertValid(f1);

			validator.GetInvalidValues(f2).Should().Not.Be.Empty();
		}

		[Test]
		public void PastAndFuture()
		{
			IClassValidator validator = GetClassValidator(typeof(FooDate));
			FooDate f = new FooDate();

			f.Past = DateTime.MinValue;
			f.Future = DateTime.MaxValue;
			validator.GetInvalidValues(f).Should().Be.Empty();

			f.Future = DateTime.Today.AddDays(1); //tomorrow
			f.Past = DateTime.Today.AddDays(-1); //yesterday
			validator.GetInvalidValues(f).Should().Be.Empty();

			f.Future = DateTime.Today.AddDays(-1); //yesterday
			validator.GetInvalidValues(f).Should().Have.Count.EqualTo(1);

			f.Future = DateTime.Today.AddDays(1); //tomorrow
			f.Past = DateTime.Today.AddDays(1); //tomorrow
			validator.GetInvalidValues(f).Should().Have.Count.EqualTo(1);

			f.Future = DateTime.Now.AddMilliseconds(-1);
			f.Past = DateTime.Now.AddMilliseconds(+1);
			validator.GetInvalidValues(f).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void NullString()
		{
			FooNotEmpty f = new FooNotEmpty(null);

			IClassValidator vtor = GetClassValidator(typeof (FooNotEmpty));

			vtor.GetInvalidValues(f).Should().Not.Be.Empty();
		}
	}
}