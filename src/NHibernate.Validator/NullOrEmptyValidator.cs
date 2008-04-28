using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Validator for any kind of IEnumerable (including string)
	/// </summary>
	public class NotNullOrEmptyValidator : IValidator
	{
		public bool IsValid(object value)
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
