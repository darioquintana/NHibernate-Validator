using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	public static class Tags
	{
		public static readonly System.Type Error = typeof (Error);
		public static readonly System.Type Warning = typeof(Warning);
	}
	public class EntityLoq
	{
		public string ValueUsingTags { get; set; }
		public string ValueWithoutTags { get; set; }
	}

	public class EntityLoqValidation: ValidationDef<EntityLoq>
	{
		public EntityLoqValidation()
		{
			Define(x => x.ValueUsingTags).NotEmpty().WithTags(Tags.Error, Tags.Warning).And.MaxLength(20).WithTags(Tags.Error);
			Define(x => x.ValueWithoutTags).NotEmpty().And.MinLength(20);
		}
	}

	[TestFixture]
	public class ValidatorEngineTaggingLoquacious
	{
		private object lengthTypeId = (new LengthAttribute()).TypeId;
		private object notEmptyTypeId = (new NotEmptyAttribute()).TypeId;
		private ValidatorEngine ve;

		[TestFixtureSetUp]
		public void CreateEngine()
		{
			var conf = new FluentConfiguration();
			conf.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			conf.Register<EntityLoqValidation, EntityLoq>();
			ve = new ValidatorEngine();
			ve.Configure(conf);
		}

		private void GivingRulesFor(string propertyName, out ITagableRule maxAttribute, out ITagableRule notEmptyAttribute)
		{
			IClassValidator cv = ve.GetClassValidator(typeof(EntityLoq));
			IEnumerable<Attribute> ma = cv.GetMemberConstraints(propertyName);
			maxAttribute = (ITagableRule)ma.First(a => a.TypeId == lengthTypeId);
			notEmptyAttribute = (ITagableRule)ma.First(a => a.TypeId == notEmptyTypeId);
		}

		[Test]
		public void MemberConstraints_HasTags()
		{
			ITagableRule maxAttribute;
			ITagableRule notEmptyAttribute;

			GivingRulesFor("ValueUsingTags", out maxAttribute, out notEmptyAttribute);

			maxAttribute.TagCollection.Should().Contain(Tags.Error);
			notEmptyAttribute.TagCollection.Should().Contain(Tags.Error).And.Contain(Tags.Warning);
		}

		[Test]
		public void MemberConstraints_DoesNotHaveTags()
		{
			ITagableRule maxAttribute;
			ITagableRule notEmptyAttribute;

			GivingRulesFor("ValueWithoutTags", out maxAttribute, out notEmptyAttribute);

			maxAttribute.TagCollection.Should().Be.Empty();
			notEmptyAttribute.TagCollection.Should().Be.Empty();
		}

		[Test]
		public void ValidateEntityWithTags()
		{
			ve.Validate(new EntityLoq {ValueUsingTags = new string('A', 21)}, Tags.Warning).Should().Be.Empty();
			ve.Validate(new EntityLoq {ValueUsingTags = ""}, Tags.Error).Should().Have.Count.EqualTo(1);
		}
	}
}