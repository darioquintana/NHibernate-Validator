using System;
using System.Runtime.Serialization;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class HibernateValidatorException : HibernateException
	{
		public HibernateValidatorException() : base("An exception occurred in the validation layer.") { }

		public HibernateValidatorException(string message) : base(message) { }

		public HibernateValidatorException(string message, System.Exception inner) : base(message, inner) { }

		protected HibernateValidatorException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}