using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DelegatedSimpleConstraint<TSubject> : IValidator
	{
		private readonly Func<TSubject, bool> isValidDelegate;

		public DelegatedSimpleConstraint(Func<TSubject, bool> isValidDelegate)
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
			return isValidDelegate((TSubject)value);
		}

		#endregion
	}
}