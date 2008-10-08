using System;
using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class NotEmptyValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value)
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
			if (ev != null)
			{
				return ev.GetEnumerator().MoveNext();
			}

			return false;
		}

		#endregion
	}
}