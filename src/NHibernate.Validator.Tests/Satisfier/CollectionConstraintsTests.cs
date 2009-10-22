using System.Linq;
using System.Collections.Generic;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Satisfier
{
	public class EntityWithCollection
	{
		public ICollection<string> Value { get; set; }
	}

	[TestFixture]
	public class CollectionConstraintsTests
	{
		[Test]
		public void Validate()
		{
			var configure = new FluentConfiguration();

			var validationDef = new ValidationDef<EntityWithCollection>();
			validationDef.Define(e => e.Value).Satisfy(v => v != null && v.Any(e => e == "something")).WithMessage("Should contain 'something'");

			configure.Register(validationDef).SetDefaultValidatorMode(ValidatorMode.UseExternal);
			var ve = new ValidatorEngine();

			ve.Configure(configure);

			Assert.That(ve.IsValid(new EntityWithCollection { Value = new[]{"b", "something"} }));
			Assert.That(!ve.IsValid(new EntityWithCollection()));
			Assert.That(!ve.IsValid(new EntityWithCollection { Value = new[] { "b" } }));
		}
	}
}