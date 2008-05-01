using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class PastValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null) return true;

			if (value is DateTime)
			{
				return DateTime.Now.CompareTo(value) > 0;
			}

			return false;
		}
	}
}