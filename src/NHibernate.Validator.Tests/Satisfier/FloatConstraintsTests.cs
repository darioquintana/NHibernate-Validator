using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Satisfier
{
	public class EntityWithFloats<T> where T : struct
	{
		public T Value { get; set; }
		public T? NullValue { get; set; }
	}

	[TestFixture]
	public class FloatConstraintsTests
	{
		[Test]
		public void ValidateFloat()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithFloats<float>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithFloats<float> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<float>()));
			Assert.That(!ve.IsValid(new EntityWithFloats<float> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<float> { NullValue = 1 }));
		}

		[Test]
		public void ValidateDouble()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithFloats<double>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithFloats<double> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<double>()));
			Assert.That(!ve.IsValid(new EntityWithFloats<double> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<double> { NullValue = 1 }));
		}

		[Test]
		public void ValidateDecimal()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithFloats<decimal>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithFloats<decimal> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<decimal>()));
			Assert.That(!ve.IsValid(new EntityWithFloats<decimal> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithFloats<decimal> { NullValue = 1 }));
		}
	}
}