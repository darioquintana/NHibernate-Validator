using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	[CLSCompliant(false)]
	public class DelegatedValidatorAttribute : EmbeddedRuleArgsAttribute, IValidatorInstanceProvider
	{
		private readonly IValidator validatorInstance;

		public DelegatedValidatorAttribute(IValidator validatorInstance)
		{
			this.validatorInstance = validatorInstance;
			this.ErrorMessage = "";
		}

		public override bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return this.validatorInstance.IsValid(value, constraintValidatorContext);
		}

		public IValidator Validator
		{
			get { return validatorInstance; }
		}
	}
}