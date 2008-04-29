using System;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

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

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
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
	}
}