using System;
using System.Collections;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class NotNullNotEmptyValidator : NotNullValidator
	{
		public override bool IsValid(object value)
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