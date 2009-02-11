using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Mappings;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	[TestFixture]
	public class ValidationDefFixture
	{
		private const BindingFlags membersBindingFlags =
			BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			| BindingFlags.Static;

		[Test]
		public void ShouldAddPropertiesValidators()
		{
			var v = new ValidationDef<KnownRules>();
			v.Define(x => x.DtProp).IsInThePast();
			IClassMapping cm = v.GetMapping();
			PropertyInfo lpi = typeof (KnownRules).GetProperty("DtProp", membersBindingFlags);

			Assert.That(cm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(cm.GetMemberAttributes(lpi).First(), Is.InstanceOf<PastAttribute>());

			var kv = new KnownRulesSimpleValidationDef();
			cm = kv.GetMapping();
			Assert.That(cm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(cm.GetMemberAttributes(lpi).First(), Is.InstanceOf<PastAttribute>());
		}

		public class KnownRulesSimpleValidationDef: ValidationDef<KnownRules>
		{
			public KnownRulesSimpleValidationDef()
			{
				Define(x => x.DtProp).IsInThePast();
			}
		}
	}
}