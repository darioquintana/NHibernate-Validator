using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (CreditCardNumberValidator))]
	public class CreditCardNumberAttribute : EmbeddedRuleArgsAttribute
	{
		public CreditCardNumberAttribute()
		{
			this.ErrorMessage = "{validator.creditCard}";
		}

		public override bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return new CreditCardNumberValidator().IsValid(value, constraintValidatorContext);
		}
	}
}