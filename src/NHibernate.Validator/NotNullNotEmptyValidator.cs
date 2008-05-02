using System.Collections;

namespace NHibernate.Validator
{
	/// <summary>
	/// Validator for any kind of IEnumerable (including string)
	/// </summary>
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
