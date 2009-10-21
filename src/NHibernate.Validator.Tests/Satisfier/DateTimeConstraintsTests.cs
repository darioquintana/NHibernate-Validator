using System;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Satisfier
{
	public class EntityWithDate
	{
		public DateTime Value { get; set; }
		public DateTime? NullValue { get; set; }
	}

	[TestFixture]
	public class DateTimeConstraintsTests
	{
		[Test]
		public void Validate()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithDate>();
			validationDef.Define(e => e.Value).Satisfy(v => v.Year == DateTime.Today.Year).WithMessage("In this year");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithDate { Value = DateTime.Today, NullValue = DateTime.Today }));
			Assert.That(!ve.IsValid(new EntityWithDate()));
			Assert.That(!ve.IsValid(new EntityWithDate { Value = DateTime.Today }));
			Assert.That(!ve.IsValid(new EntityWithDate { NullValue = DateTime.Today }));
		}
	}
}