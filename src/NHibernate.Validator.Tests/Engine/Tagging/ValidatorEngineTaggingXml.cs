using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;
using Environment = NHibernate.Validator.Cfg.Environment;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	public class EntityXml
	{
		public int ValueUsingTags { get; set; }
		public int ValueWithoutTags { get; set; }
	}

	[TestFixture]
	public class ValidatorEngineTaggingXml
	{
		private object minTypeId = (new MinAttribute()).TypeId;
		private object maxTypeId = (new MaxAttribute()).TypeId;
		private ValidatorEngine ve;

		[TestFixtureSetUp]
		public void CreateEngine()
		{
			var conf = new NHVConfigurationBase();
			conf.Properties[Environment.ValidatorMode] = "UseExternal";
			conf.Mappings.Add(new MappingConfiguration("NHibernate.Validator.Tests", "NHibernate.Validator.Tests.Engine.Tagging.EntityXml.nhv.xml"));
			ve = new ValidatorEngine();
			ve.Configure(conf);
		}

		private void GivingRulesFor(string propertyName, out ITagableRule minAttribute, out ITagableRule maxAttribute)
		{
			IClassValidator cv = ve.GetClassValidator(typeof (EntityXml));
			IEnumerable<Attribute> ma = cv.GetMemberConstraints(propertyName);
			minAttribute = (ITagableRule)ma.First(a => a.TypeId == minTypeId);
			maxAttribute = (ITagableRule)ma.First(a => a.TypeId == maxTypeId);
		}

		[Test]
		public void MemberConstraints_HasTags()
		{
			ITagableRule minAttribute;
			ITagableRule maxAttribute;

			GivingRulesFor("ValueUsingTags", out minAttribute, out maxAttribute);

			minAttribute.TagCollection.Should().Contain("error");
			maxAttribute.TagCollection.Should().Contain("error").And.Contain("warning");
		}

		[Test]
		public void MemberConstraints_DoesNotHaveTags()
		{
			ITagableRule minAttribute;
			ITagableRule maxAttribute;

			GivingRulesFor("ValueWithoutTags", out minAttribute, out maxAttribute);

			minAttribute.TagCollection.Should().Be.Empty();
			maxAttribute.TagCollection.Should().Be.Empty();
		}

	}
}