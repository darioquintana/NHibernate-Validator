using System;
using System.Runtime.Serialization;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class ValidatorConfigurationException : HibernateValidatorException
	{
		private const string baseMessage = "An exception occurred during configuration of Validation framework.";

		public ValidatorConfigurationException() { }

		public ValidatorConfigurationException(string message) : base(message) { }

		public ValidatorConfigurationException(string message, System.Exception inner) : base(message, inner) { }

		public ValidatorConfigurationException(Exception innerException)
			: base(baseMessage, innerException) { }

		protected ValidatorConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}