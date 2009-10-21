using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Satisfier
{
	public class EntityWithString
	{
		public string Name { get; set; }
	}

	[TestFixture]
	public class StringConstraintsTests
	{
		[Test]
		public void ValidateWithSingleCondition()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithString>();
			validationDef.Define(e => e.Name).Satisfy(name => name != null && name.StartsWith("ab")).WithMessage(
				"Name should start with 'ab'");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithString {Name = "abc"}));
			Assert.That(!ve.IsValid(new EntityWithString { Name = "bc" }));
		}

		[Test]
		public void ValidateWithMultipleConditions()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithString>();
			validationDef.Define(e => e.Name)
				.Satisfy(name => name != null && name.StartsWith("ab")).WithMessage("Name should start with 'ab'")
				.And
				.Satisfy(name => name != null && name.EndsWith("zz")).WithMessage("Name should end with 'zz'");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithString { Name = "abczz" }));
			Assert.That(!ve.IsValid(new EntityWithString { Name = "bc" }));
			var iv = ve.Validate(new EntityWithString {Name = "abc"});
			Assert.That(iv.Length, Is.EqualTo(1));
			Assert.That(iv.Select(i => i.Message).First(), Is.EqualTo("Name should end with 'zz'"));

			iv = ve.Validate(new EntityWithString { Name = "zz" });
			Assert.That(iv.Length, Is.EqualTo(1));
			Assert.That(iv.Select(i => i.Message).First(), Is.EqualTo("Name should start with 'ab'"));

			iv = ve.Validate(new EntityWithString { Name = "bc" });
			Assert.That(iv.Length, Is.EqualTo(2));
			var messages = iv.Select(i => i.Message);
			Assert.That(messages, Has.Member("Name should start with 'ab'") & Has.Member("Name should end with 'zz'"));
		}
	}
}