using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	public class Error { }
	public class Warning { }

	public enum MyEnum
	{
		Error,
		Warining
	}

	public class Entity
	{
		[Min(Value = 20, Tags = typeof(Error), Message = "a message")]
		[Max(Tags = new[] { typeof(Error), typeof(Warning) }, Value = 100)]
		public int ValueUsingTypes { get; set; }

		[Min(Value = 20, Tags = "error", Message = "a message")]
		[Max(Tags = new[] { "error", "warning" }, Value = 100)]
		public int ValueUsingStrings { get; set; }

		[Min(Value = 20, Tags = MyEnum.Error, Message = "a message")]
		[Max(Tags = new[] { MyEnum.Error, MyEnum.Warining }, Value = 100)]
		public int ValueUsingEnums { get; set; }

		[Min(20)]
		[Max(100)]
		public int ValueWithoutTags { get; set; }
	}

	public class EntityWithReletion
	{
		[Valid]
		public Entity Reference { get; set; }
	}

	public class EntityWithCollection
	{
		[Valid]
		public IEnumerable<Entity> Entities { get; set; }
	}

	[TestFixture]
	public class ClassValidatorTagging
	{
		private object minTypeId = (new MinAttribute()).TypeId;
		private object maxTypeId = (new MaxAttribute()).TypeId;

		private void GivingRulesFor(string propertyName, out ITagableRule minAttribute, out ITagableRule maxAttribute)
		{
			IClassValidator cv = new ClassValidator(typeof (Entity));
			IEnumerable<Attribute> ma = cv.GetMemberConstraints(propertyName);
			minAttribute = (ITagableRule) ma.First(a => a.TypeId == minTypeId);
			maxAttribute = (ITagableRule) ma.First(a => a.TypeId == maxTypeId);
		}

		[Test]
		public void MemberConstraints_ByTagOfType()
		{
			ITagableRule minAttribute;
			ITagableRule maxAttribute;
			
			GivingRulesFor("ValueUsingTypes", out minAttribute, out maxAttribute);

			minAttribute.TagCollection.Should().Contain(typeof (Error));
			maxAttribute.TagCollection.Should().Contain(typeof (Error)).And.Contain(typeof (Warning));
		}

		[Test]
		public void MemberConstraints_ByTagOfStrings()
		{
			ITagableRule minAttribute;
			ITagableRule maxAttribute;

			GivingRulesFor("ValueUsingStrings", out minAttribute, out maxAttribute);

			minAttribute.TagCollection.Should().Contain("error");
			maxAttribute.TagCollection.Should().Contain("error").And.Contain("warning");
		}

		[Test]
		public void MemberConstraints_ByTagOfEnums()
		{
			ITagableRule minAttribute;
			ITagableRule maxAttribute;

			GivingRulesFor("ValueUsingEnums", out minAttribute, out maxAttribute);

			minAttribute.TagCollection.Should().Contain(MyEnum.Error);
			maxAttribute.TagCollection.Should().Contain(MyEnum.Error).And.Contain(MyEnum.Warining);
		}

		[Test]
		public void UseOnlySpecificTaggedValidators()
		{
			IClassValidator cv = new ClassValidator(typeof (Entity));
			cv.GetInvalidValues(new Entity {ValueUsingTypes = 5}, "ValueUsingTypes", typeof (Warning)).Should().Have.Count.
				EqualTo(0);
			cv.GetInvalidValues(new Entity { ValueUsingTypes = 101}, "ValueUsingTypes", typeof(Warning)).Should().Have.Count.
				EqualTo(1);
			cv.GetInvalidValues(new Entity { ValueUsingTypes = 5 }, "ValueUsingTypes", typeof(Error)).Should().Have.Count.
				EqualTo(1);
			cv.GetInvalidValues(new Entity { ValueUsingTypes = 101 }, "ValueUsingTypes", typeof(Error)).Should().Have.Count.
				EqualTo(1);

			// Mixing
			cv.GetInvalidValues(new Entity(), typeof (Error), "error").Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void WhenNoTagIsSpecified_ValidateAnything()
		{
			// all propeties are wrong
			IClassValidator cv = new ClassValidator(typeof(Entity));
			cv.GetInvalidValues(new Entity()).Should().Have.Count.EqualTo(4);
		}

		[Test]
		public void WhenTagIsSpecified_ValidateOnlyWithTag()
		{
			// only property with 'typeof(Error)' as tag
			IClassValidator cv = new ClassValidator(typeof(Entity));
			cv.GetInvalidValues(new Entity(), typeof(Error)).Should().Have.Count.EqualTo(1);
		}

		[Test]
		public void WhenTagsIncludeNull_ValidateOnlyWithTagAndWithNoTags()
		{
			// only properties with 'typeof(Error)' as tag and with no tag (prop 'ValueWithoutTags')
			IClassValidator cv = new ClassValidator(typeof(Entity));
			cv.GetInvalidValues(new Entity(), typeof(Error), null).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void WhenTagsIncludeOnlyNull_ValidateOnlyWithNoTags()
		{
			// only property 'ValueWithoutTags'
			IClassValidator cv = new ClassValidator(typeof(Entity));
			cv.GetInvalidValues(new Entity(), new object[] {null}).Should().Have.Count.EqualTo(1);
		}

		[Test]
		public void WhenTagIsSpecified_ValidateRelationForGivenTags()
		{
			// specifying [Valid] the relation is always analized
			IClassValidator cv = new ClassValidator(typeof(EntityWithReletion));
			cv.GetInvalidValues(new EntityWithReletion { Reference = new Entity() }, typeof(Error)).Should().Have.Count.EqualTo(1);
			cv.GetInvalidValues(new EntityWithReletion { Reference = new Entity() }, typeof(Error), null).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void WhenTagIsSpecified_ValidateCollectionForGivenTags()
		{
			// specifying [Valid] the collection is always analized
			IClassValidator cv = new ClassValidator(typeof(EntityWithCollection));
			cv.GetInvalidValues(new EntityWithCollection { Entities = new List<Entity> { new Entity(), new Entity() } }, typeof(Error)).Should().Have.Count.EqualTo(2);
			cv.GetInvalidValues(new EntityWithCollection { Entities = new List<Entity> { new Entity(), new Entity() } }, typeof(Error), null).Should().Have.Count.EqualTo(4);
		}

		[Test]
		public void ProofFor_NHV48()
		{
			IClassValidator cv = new ClassValidator(typeof(Book));
			cv.GetInvalidValues(new Book(), BTags.Draft).Should().Have.Count.EqualTo(1);
			cv.GetInvalidValues(new Book(), BTags.Publish).Should().Have.Count.EqualTo(2);
		}
	}

	public class Draft { }
	public class Publish { }

	public static class BTags
	{
		public static System.Type Draft = typeof (Draft);
		public static System.Type[] Publish = new[] { typeof(Publish), typeof(Draft) };
	}
	public class Book
	{
		[NotNullNotEmpty(Tags = typeof(Draft))]
		public string Title { get; set; }

		[NotNullNotEmpty(Tags = typeof(Publish))]
		public string Abstract { get; set; }
	}

}