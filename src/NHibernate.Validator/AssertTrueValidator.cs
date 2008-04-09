using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class AssertTrueValidator : IValidator
	{
		public bool IsValid(Object value)
		{
			return (bool) value;
		}
	}
}