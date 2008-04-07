using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using log4net;

namespace NHibernate.Validator.Tests.XmlAndAttributesMix
{
	[TestFixture]
	public class OverrideAttributeWithXmlFixture : BaseValidatorFixture
	{
		private static ILog log = LogManager.GetLogger(typeof(OverrideAttributeWithXmlFixture));

		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForOverrideAttributeWithXml(type);
		}

		[Test]
		public void AttributeLengthForAddressOverridesAttribute()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "aaa";
			person.friends = 2;

			ClassValidator validator = GetClassValidator(typeof(Person));
			InvalidValue[] invalids = validator.GetInvalidValues(person);
			Assert.AreEqual(0, invalids.Length, "Address has not minimum");
		}

		[Test]
		public void GetCombinedDefinition()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "Address";
			person.friends = 21;

			ClassValidator validator = GetClassValidator(typeof(Person));
			InvalidValue[] invalids = validator.GetInvalidValues(person);
			Assert.AreEqual(1, invalids.Length, "Person cannot have more than 20 friends by Xml");

			person.friends = 1;
			invalids = validator.GetInvalidValues(person);
			Assert.AreEqual(1, invalids.Length, "Person cannot have less than 2 friends by Attribute");
		}

		[Test]
		public void GetAttributeOnlyDefinition()
		{
			Person person = new Person();
			person.Name = string.Empty;
			person.IsMale = true;
			person.Address = "Address";
			person.friends = 2;

			ClassValidator validator = GetClassValidator(typeof(Person));
			InvalidValue[] invalids = validator.GetInvalidValues(person);
			Assert.AreEqual(1, invalids.Length, "Name cannot be empty by attribute");
		}
	}
}
