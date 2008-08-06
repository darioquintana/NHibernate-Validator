using System;
using System.Collections;

namespace NHibernate.Validator
{
	[Serializable]
	public class NotNullNotEmptyValidator : NotNullValidator
	{
		public override bool IsValid(object value)
		{
			IEnumerable ev = value as IEnumerable;
			if (ev != null)
			{
				return ev.GetEnumerator().MoveNext();
			}

			return false;
		}
	}
}
