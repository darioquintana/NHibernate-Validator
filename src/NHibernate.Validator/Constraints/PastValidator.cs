using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class PastValidator : IValidator
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
				return DateTime.Now.CompareTo(value) > 0;
			}

			return false;
		}

		#endregion
	}
}