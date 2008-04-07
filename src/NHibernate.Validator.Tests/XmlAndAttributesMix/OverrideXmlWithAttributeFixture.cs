using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.XmlAndAttributesMix
{
	[TestFixture]
	public class OverrideXmlWithAttributeFixture : BaseValidatorFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForOverrideXmlWithAttribute(type);
		}

		[Test]
		public void AttributeLengthForAddressOverridesXml()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "aaa";
			person.friends = 2;

			ClassValidator validator = GetClassValidator(typeof(Person));

			InvalidValue[] invalids = validator.GetInvalidValues(person);

			Assert.AreEqual(1, invalids.Length, " Address length was short than 5 as set in the attribute");
		}
	}
}
