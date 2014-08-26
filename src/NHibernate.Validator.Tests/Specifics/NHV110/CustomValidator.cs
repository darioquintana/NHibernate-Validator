using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Specifics.NHV110
{
	public class CustomValidatorAttribute : EmbeddedRuleArgsAttribute
	{
		public CustomValidatorAttribute()
		{
			Message = "Error message here";
		}

		public override bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return false;
		}
	}
}