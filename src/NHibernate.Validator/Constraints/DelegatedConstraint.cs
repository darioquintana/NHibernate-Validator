using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DelegatedConstraint<TEntity> : IValidator where TEntity: class
	{
		private readonly Func<TEntity, IConstraintValidatorContext, bool> isValidDelegate;

		public DelegatedConstraint(Func<TEntity, IConstraintValidatorContext, bool> isValidDelegate)
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
			return isValidDelegate(value as TEntity, constraintValidatorContext);
		}

		#endregion
	}
}