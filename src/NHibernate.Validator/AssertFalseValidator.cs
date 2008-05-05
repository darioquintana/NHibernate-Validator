using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class AssertFalseValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null)
				return false;

			if (value is bool)
				return !(bool)value;

			return false;
		}
	}
}
