using System;
using System.Collections;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class NotEmptyValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			var check = value as string;
			if (check != null)
			{
				return !string.Empty.Equals(check.Trim());
			}

			var ev = value as IEnumerable;
			return ev != null && ev.Any();
		}

		#endregion
	}
}