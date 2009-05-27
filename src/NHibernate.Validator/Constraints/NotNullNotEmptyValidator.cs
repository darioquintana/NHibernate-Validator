using System;
using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class NotNullNotEmptyValidator : NotNullValidator
	{
		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			var ev = value as IEnumerable;
			if (ev != null)
			{
				return ev.GetEnumerator().MoveNext();
			}

			return false;
		}
	}
}