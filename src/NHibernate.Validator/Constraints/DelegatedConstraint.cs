using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DelegatedConstraint<TSubject> : IValidator
	{
		private readonly Func<TSubject, IConstraintValidatorContext, bool> isValidDelegate;

		public DelegatedConstraint(Func<TSubject, IConstraintValidatorContext, bool> isValidDelegate)
		{
			if (isValidDelegate == null)
			{
				throw new ArgumentNullException("isValidDelegate");
			}
			this.isValidDelegate = isValidDelegate;
		}

		#region Implementation of IValidator

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return isValidDelegate((TSubject)value, constraintValidatorContext);
		}

		#endregion
	}
}