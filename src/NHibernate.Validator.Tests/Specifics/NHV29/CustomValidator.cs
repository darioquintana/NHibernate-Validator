using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Specifics.NHV29
{
	[ValidatorClass(typeof (CustomValidator))]
	public class CustomValidatorAttribute : Attribute, IRuleArgs
	{
		public CustomValidatorAttribute()
		{
			Message = "Error when validating with CustomValidator";
		}

		public string Message { get; set; }
	}

	/// <summary>
	/// Valid for Not-Null values.
	/// </summary>
	public class CustomValidator : IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			var stringValue = (string) value;

			return !string.IsNullOrEmpty(stringValue);
		}
	}
}