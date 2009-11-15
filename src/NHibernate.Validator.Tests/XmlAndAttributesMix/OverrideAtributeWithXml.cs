using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using log4net;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.XmlAndAttributesMix
{
	[TestFixture]
	public class OverrideAttributeWithXmlFixture : BaseValidatorFixture
	{
		private static ILog log = LogManager.GetLogger(typeof(OverrideAttributeWithXmlFixture));

		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForOverrideAttributeWithExternal(type);
		}

		[Test]
		public void AttributeLengthForAddressOverridesAttribute()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "aaa";
			person.friends = 2;

			IClassValidator validator = GetClassValidator(typeof(Person));
			validator.GetInvalidValues(person).Should("Address has not minimum").Be.Empty();
		}

		[Test]
		public void GetCombinedDefinition()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "Address";
			person.friends = 21;

			IClassValidator validator = GetClassValidator(typeof(Person));
			validator.GetInvalidValues(person).Should("Person cannot have more than 20 friends by Xml").Have.Count.EqualTo(1);

			person.friends = 1;
			validator.GetInvalidValues(person).Should("Person cannot have less than 2 friends by Attribute").Have.Count.EqualTo(1);
		}

		[Test]
		public void GetAttributeOnlyDefinition()
		{
			Person person = new Person();
			person.Name = string.Empty;
			person.IsMale = true;
			person.Address = "Address";
			person.friends = 2;

			IClassValidator validator = GetClassValidator(typeof(Person));
			validator.GetInvalidValues(person).Should("Name cannot be empty by attribute").Have.Count.EqualTo(1);
		}
	}
}
