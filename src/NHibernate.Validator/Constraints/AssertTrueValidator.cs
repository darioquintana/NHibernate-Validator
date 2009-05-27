using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class AssertTrueValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(Object value, IConstraintValidatorContext constraintContext)
		{
			return (bool) value;
		}

		#endregion
	}
}