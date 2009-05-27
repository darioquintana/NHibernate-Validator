using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Mappings
{
	[AttributeUsage(AttributeTargets.Class)]
	[ValidatorClass(typeof(OtherValidator))]
	public class OtherValidatorAttribute : Attribute
	{

	}

	public class OtherValidator : IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			return false;
		}
	}
}