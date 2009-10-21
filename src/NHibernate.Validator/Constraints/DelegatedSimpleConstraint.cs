using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class DelegatedSimpleConstraint<TEntity> : IValidator where TEntity: class
	{
		private readonly Func<TEntity, bool> isValidDelegate;

		public DelegatedSimpleConstraint(Func<TEntity, bool> isValidDelegate)
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
			return isValidDelegate(value as TEntity);
		}

		#endregion
	}
}