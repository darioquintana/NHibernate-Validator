using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class AssertFalseValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return false;
			}

			if (value is bool)
			{
				return !(bool) value;
			}

			return false;
		}

		#endregion
	}
}