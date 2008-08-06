using System;
using System.Collections;

namespace NHibernate.Validator
{
	/// <summary>
	/// Ensure a IEnumerable (including string) to be not null and not empty.
	/// <code>
	/// <example>
	/// Valid values: "abc"; new int[] {1}; new List<int>(new int[] { 1 });
	/// Invalid values: null; ""; 123; new int[0]; new List<int>();
	/// </example>
	/// </code>
	/// </summary>
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
