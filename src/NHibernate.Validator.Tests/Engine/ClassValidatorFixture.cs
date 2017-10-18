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
		public void CreationOfValidatorForSystemTypeShouldThrow()
		{
			Assert.That(() => new ClassValidator(typeof(string)), Throws.TypeOf<ArgumentOutOfRangeException>());
				//.ActualValue.Should().Be.EqualTo(typeof(string));
		}

		[Test]
		public void GetInvalidValuesOfEntity()
		{
			IClassValidator cv = GetClassValidator(typeof(Address));
			cv.GetInvalidValues(null).Should().Be.Empty();

			Assert.That(() => cv.GetInvalidValues(new Suricato()), Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void GetInvalidValuesOfProperty()
		{
			IClassValidator cv = GetClassValidator(typeof(Address));
			cv.GetInvalidValues(null, "blacklistedZipCode").Should().Be.Empty();
			Assert.That(() => cv.GetInvalidValues(new Suricato(), "blacklistedZipCode"), Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void CrazyUseOrNHVFault()
		{
			// In case of NHV we are prevent unecessary validations
			Assert.That(() => new ClassValidator(typeof(string)), Throws.TypeOf<ArgumentOutOfRangeException>());
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
