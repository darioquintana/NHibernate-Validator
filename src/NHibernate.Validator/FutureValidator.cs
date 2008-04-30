using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Check that a given date is in the future.
	/// </summary>
	[Serializable]
	public class FutureValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null) return true;

			if (value is DateTime)
			{
				return DateTime.Now.CompareTo(value) <= 0;
			}

			return false;
		}
	}
}