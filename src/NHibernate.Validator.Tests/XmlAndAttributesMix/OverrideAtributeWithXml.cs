using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.XmlAndAttributesMix
{
	[TestFixture]
	public class OverrideAttributeWithXmlFixture : BaseValidatorFixture
	{
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

			ClassValidator validator = GetClassValidator(typeof(Person));
			InvalidValue[] invalids = validator.GetInvalidValues(person);
			Assert.AreEqual(0, invalids.Length, "Address has not minimum");
		}
	}
}
