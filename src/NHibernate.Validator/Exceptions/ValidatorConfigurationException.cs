using System;
using System.Runtime.Serialization;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class ValidatorConfigurationException : HibernateValidatorException
	{
		public ValidatorConfigurationException() { }

		public ValidatorConfigurationException(string message) : base(message) { }

		public ValidatorConfigurationException(string message, System.Exception inner) : base(message, inner) { }

		protected ValidatorConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}