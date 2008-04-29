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
			Attribute found = RuleAttributeFactory.CreateAttributeFromClass(typeof (Suricato), "AssertAnimal");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof (AssertAnimalAttribute), found.GetType());

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof (Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof (AssertAnimalAttribute), found.GetType());

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof (Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
		}

		[Test, ExpectedException(typeof (InvalidAttributeNameException))]
		public void CreateAttributeFromClassWrongName()
		{
			RuleAttributeFactory.CreateAttributeFromClass(typeof (Suricato), "assertanimal");
		}
	}
}