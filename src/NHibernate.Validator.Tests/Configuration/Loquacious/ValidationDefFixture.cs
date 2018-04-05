using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Mappings;
using NUnit.Framework;
using SharpTestsEx;

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
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			PropertyInfo lpi = typeof(KnownRules).GetProperty("DtProp", membersBindingFlags);

			Assert.That(cm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(cm.GetMemberAttributes(lpi).First(), Is.InstanceOf<PastAttribute>());

			var kv = new KnownRulesSimpleValidationDef();
			cm = ((IMappingSource)kv).GetMapping();
			Assert.That(cm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(cm.GetMemberAttributes(lpi).First(), Is.InstanceOf<PastAttribute>());
		}

		[Test]
		public void ShouldAssignRuleArgsOptions()
		{
			PropertyInfo lpi = typeof(KnownRules).GetProperty("DtProp", membersBindingFlags);
			var v = new ValidationDef<KnownRules>();
			string expected = "{validator.past}";
			v.Define(x => x.DtProp).IsInThePast();
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			Assert.That(cm.GetMemberAttributes(lpi).OfType<PastAttribute>().First().Message, Is.EqualTo(expected));

			v = new ValidationDef<KnownRules>();
			expected = "The date is in the past.";
			v.Define(x => x.DtProp).IsInThePast().WithMessage(expected);
			cm = ((IMappingSource)v).GetMapping();
			Assert.That(cm.GetMemberAttributes(lpi).OfType<PastAttribute>().First().Message, Is.EqualTo(expected));
		}

		[Test]
		public void ShouldWorkWithStringConstraint()
		{
			PropertyInfo lpi = typeof(KnownRules).GetProperty("StrProp", membersBindingFlags);

			var v = new ValidationDef<KnownRules>();
			var expectedMessage = "The StrProp is too long {Max}";
			v.Define(x => x.StrProp).MaxLength(10).WithMessage(expectedMessage).And.NotNullable().And.NotEmpty();
			IClassMapping cm = ((IMappingSource)v).GetMapping();

			var mAttrs = cm.GetMemberAttributes(lpi);
			Assert.That(mAttrs.Count(), Is.EqualTo(3));
			var lengthAttribute = mAttrs.OfType<LengthAttribute>().FirstOrDefault();
			Assert.That(lengthAttribute, Is.Not.Null);
			Assert.That(lengthAttribute.Max, Is.EqualTo(10));
			Assert.That(lengthAttribute.Message, Is.EqualTo(expectedMessage));
			Assert.That(mAttrs.OfType<NotNullAttribute>().FirstOrDefault(), Is.Not.Null);
			Assert.That(mAttrs.OfType<NotEmptyAttribute>().FirstOrDefault(), Is.Not.Null);

			v = new ValidationDef<KnownRules>();
			v.Define(x => x.StrProp).NotNullable().And.IsEmail();
			cm = ((IMappingSource)v).GetMapping();
			mAttrs = cm.GetMemberAttributes(lpi);
			Assert.That(mAttrs.OfType<EmailAttribute>().FirstOrDefault(), Is.Not.Null);
			Assert.That(mAttrs.OfType<NotNullAttribute>().FirstOrDefault(), Is.Not.Null);
		}

		[Test]
		public void ShouldWorkWithCollectionConstraints()
		{
			PropertyInfo lpi = typeof(KnownRules).GetProperty("ArrProp", membersBindingFlags);
			var v = new ValidationDef<KnownRules>();
			v.Define(x => x.ArrProp).SizeBetween(1, 9);
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			var mAttrs = cm.GetMemberAttributes(lpi);
			Assert.That(mAttrs.Count(), Is.EqualTo(1));

			v = new ValidationDef<KnownRules>();
			v.Define(x => x.ArrProp).NotNullable().And.SizeBetween(1, 9);
			cm = ((IMappingSource)v).GetMapping();
			mAttrs = cm.GetMemberAttributes(lpi);
			Assert.That(mAttrs.Count(), Is.EqualTo(2));
		}

		[Test]
		public void ShouldWorkWithEnum()
		{
			var v = new ValidationDef<KnownRules>();
			Assert.That(() => v.Define(x => x.Enum), Throws.Nothing);
		}

		[Test]
		public void ShouldWorkWithGuid()
		{
			PropertyInfo lpi = typeof(KnownRules).GetProperty("GuidProp", membersBindingFlags);
			var v = new ValidationDef<KnownRules>();
			v.Define(x => x.GuidProp).NotEmpty();
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			cm.GetMemberAttributes(lpi).Select(x => x.GetType()).Single().Should().Be.EqualTo<NotNullNotEmptyAttribute>();
		}
	}
}