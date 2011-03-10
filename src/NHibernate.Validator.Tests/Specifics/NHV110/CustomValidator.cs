using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Specifics.NHV110
{
	public class CustomValidatorAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		public CustomValidatorAttribute()
		{
			Message = "Error message here";
		}

		public string Message { get; set; }

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return false;
		}
	}
}