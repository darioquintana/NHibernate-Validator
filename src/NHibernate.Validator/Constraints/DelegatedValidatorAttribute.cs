using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class DelegatedValidatorAttribute : Attribute, IValidatorInstanceProvider, IRuleArgs
	{
		private readonly IValidator validatorInstance;

		public DelegatedValidatorAttribute(IValidator validatorInstance)
		{
			this.validatorInstance = validatorInstance;
			Message = "";
		}

		public string Message { get; set; }

		public IValidator Validator
		{
			get { return validatorInstance; }
		}
	}
}