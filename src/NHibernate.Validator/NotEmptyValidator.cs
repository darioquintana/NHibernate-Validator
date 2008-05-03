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

			string check = value as string;
			if(check != null)
			{
				return !string.Empty.Equals(check.Trim());
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