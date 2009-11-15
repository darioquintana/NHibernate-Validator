using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.XmlAndAttributesMix
{
	[TestFixture]
	public class OverrideXmlWithAttributeFixture : BaseValidatorFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForOverrideExternalWithAttribute(type);
		}

		[Test]
		public void AttributeLengthForAddressOverridesXml()
		{
			Person person = new Person();
			person.Name = "MyName";
			person.IsMale = true;
			person.Address = "aaa";
			person.friends = 2;

			IClassValidator validator = GetClassValidator(typeof(Person));

			validator.GetInvalidValues(person).Should(" Address length was short than 5 as set in the attribute").Not.Be.Empty();
		}
	}
}
