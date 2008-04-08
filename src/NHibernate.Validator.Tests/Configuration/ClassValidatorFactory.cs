using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests
{
	public class ClassValidatorFactory
	{
		public static ClassValidator GetValidatorForUseXmlTest(System.Type type)
		{
			return CreateValidator(type, ValidatorMode.UseXml);
		}

		private static ClassValidator CreateValidator(System.Type type, ValidatorMode mode)
		{
			return new ClassValidator(type,
											new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
											Assembly.GetExecutingAssembly()),
											new CultureInfo("en"),
											mode);
		}

		internal static ClassValidator GetValidatorForUseAttributeTest(System.Type type)
		{
			return CreateValidator(type, ValidatorMode.UseAttribute);
		}

		internal static ClassValidator GetValidatorForOverrideXmlWithAttribute(System.Type type)
		{
			return CreateValidator(type, ValidatorMode.OverrideXmlWithAttribute);
		}

		internal static ClassValidator GetValidatorForOverrideAttributeWithXml(System.Type type)
		{
			return CreateValidator(type, ValidatorMode.OverrideAttributeWithXml);
		}
	}
}
