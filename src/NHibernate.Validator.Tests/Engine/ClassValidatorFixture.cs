using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using NHibernate.Validator.Constraints;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class ClassValidatorFixture : BaseValidatorFixture
	{
		// this Fixture is to test some special case

		[Test]
		public void GetInvalidValuesOfEntity()
		{
			IClassValidator cv = GetClassValidator(typeof(Address));
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
			IClassValidator cv = GetClassValidator(typeof(Address));
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

		[Test]
		public void CrazyUseOrNHVFault()
		{
			// In case of NHV we are prevent unecessary validations
			ActionAssert.Throws<ArgumentOutOfRangeException>(() => new ClassValidator(typeof(string)));
		}

		[Test]
		public void GetMemberConstraints()
		{
			IClassValidator cv = GetClassValidator(typeof(Address));
			IEnumerable<Attribute> ma = cv.GetMemberConstraints("country");
			Assert.That(ma.Count(), Is.EqualTo(2));
			Assert.That(ma.Count(x => x.TypeId == (new NotNullAttribute()).TypeId), Is.EqualTo(1));
			Assert.That(ma.Count(x => x.TypeId == (new LengthAttribute()).TypeId), Is.EqualTo(1));
		}

		[Test]
		public void GetMemberConstraintsLoquacious()
		{
			var configure = new FluentConfiguration();

			configure.Register(
				Assembly.GetExecutingAssembly().ValidationDefinitions().Where(x=>x.Equals(typeof(AddressDef))))
			.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			IClassValidator cv = ve.GetValidator<Address>();
			IEnumerable<Attribute> ma = cv.GetMemberConstraints("Country");
			Assert.That(ma.Count(), Is.EqualTo(2));
			Assert.That(ma.Count(x => x.TypeId == (new NotNullAttribute()).TypeId), Is.EqualTo(1));
			Assert.That(ma.Count(x => x.TypeId == (new LengthAttribute()).TypeId), Is.EqualTo(1));
		}
	}
}
