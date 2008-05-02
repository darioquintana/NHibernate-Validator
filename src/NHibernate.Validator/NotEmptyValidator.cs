using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class NotEmptyValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			IEnumerable ev = value as IEnumerable;
			if (ev != null)
			{
				return ev.GetEnumerator().MoveNext();
			}

			return false;
		}
	}
}