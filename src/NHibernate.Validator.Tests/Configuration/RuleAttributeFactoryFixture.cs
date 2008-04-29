using System;
using System.Collections.Generic;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using NHibernate.Validator.Cfg;
using System.Reflection;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class RuleAttributeFactoryFixture
	{
		[Test]
		public void CreateAttributeFromClass()
		{
			Attribute found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimal");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());
		}

		[Test, ExpectedException(typeof(InvalidAttributeNameException))]
		public void CreateAttributeFromClassWrongName()
		{
			RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "assertanimal");
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void CreateAttributeFromRule()
		{
			// Testing exception when a new element is added in XSD but not in factory
			RuleAttributeFactory.CreateAttributeFromRule("AnyObject", "", "");

			// For wellKnownRules we can't do a sure tests because we don't have a way to auto-check all
			// classes derivered from serialization.
		}

		[Test]
		public void KnownRulesConvertAssing()
		{
			NhvMapping map = MappingLoader.GetMappingFor(typeof(WellKnownRules));
			NhvmClass cm = map.@class[0];
			XmlClassMapping rm = new XmlClassMapping(cm);
			MemberInfo mi;
			List<Attribute> attr;

			mi = typeof(WellKnownRules).GetField("AP");
			attr = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual("A string value", ((ACustomAttribute) attr[0]).Value1);
			Assert.AreEqual(123, ((ACustomAttribute)attr[0]).Value2);
			Assert.AreEqual("custom message", ((ACustomAttribute)attr[0]).Message);
		}
	}
}