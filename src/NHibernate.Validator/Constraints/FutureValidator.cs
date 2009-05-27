using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a given date is in the future.
	/// </summary>
	[Serializable]
	public class FutureValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			if (value is DateTime)
			{
				return DateTime.Now.CompareTo(value) < 0;
			}

			return false;
		}

		#endregion
	}
}