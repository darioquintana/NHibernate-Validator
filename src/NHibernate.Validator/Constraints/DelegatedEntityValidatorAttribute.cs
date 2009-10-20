using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DelegatedEntityValidatorAttribute : Attribute, IValidatorInstanceProvider, IRuleArgs
	{
		private readonly IValidator validatorInstance;

		public DelegatedEntityValidatorAttribute(IValidator validatorInstance)
		{
			this.validatorInstance = validatorInstance;
		}

		public string Message { get; set; }

		public IValidator Validator
		{
			get { return validatorInstance; }
		}
	}
}