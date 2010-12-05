using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidStateException : HibernateException
	{
		private readonly InvalidValue[] _invalidValues;

		public InvalidStateException(string message, Exception inner) : base(message, inner) { }

		public InvalidStateException(InvalidValue[] invalidValues)
			: this(invalidValues, invalidValues[0].GetType().Name)
		{
		}

		public InvalidStateException(InvalidValue[] invalidValues, String className)
			: base("validation failed for: " + className)
		{
			_invalidValues = invalidValues;
		}
        
		public InvalidValue[] GetInvalidValues() 
		{
			return _invalidValues;
		}
	}
}