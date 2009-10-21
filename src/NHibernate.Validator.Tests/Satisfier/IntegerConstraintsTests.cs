using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Satisfier
{
	public class EntityWithIntegers<T> where T : struct
	{
		public T Value { get; set; }
		public T? NullValue { get; set; }
	}

	[TestFixture]
	public class IntegerConstraintsTests
	{
		[Test]
		public void ValidateShorts()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<short>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<short> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<short>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<short> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<short> { NullValue = 1 }));
		}

		[Test]
		public void ValidateInt()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<int>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<int> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<int>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<int> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<int> { NullValue = 1 }));
		}

		[Test]
		public void ValidateLong()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<long>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<long> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<long>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<long> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<long> { NullValue = 1 }));
		}

		[Test]
		public void ValidateUShorts()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<ushort>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<ushort> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ushort>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ushort> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ushort> { NullValue = 1 }));
		}

		[Test]
		public void ValidateUInt()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<uint>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<uint> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<uint>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<uint> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<uint> { NullValue = 1 }));
		}

		[Test]
		public void ValidateULong()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithIntegers<ulong>>();
			validationDef.Define(e => e.Value).Satisfy(v => v > 0).WithMessage("Value > 0");
			validationDef.Define(e => e.NullValue).Satisfy(v => v.HasValue).WithMessage("HasValue");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithIntegers<ulong> { Value = 1, NullValue = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ulong>()));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ulong> { Value = 1 }));
			Assert.That(!ve.IsValid(new EntityWithIntegers<ulong> { NullValue = 1 }));
		}
	}
}