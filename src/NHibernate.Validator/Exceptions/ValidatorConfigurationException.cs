using System;
using System.Runtime.Serialization;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class ValidatorConfigurationException : HibernateValidatorException
	{
		private const string baseMessage = "An exception occurred during configuration of Validation framework.";

		public ValidatorConfigurationException() : base(baseMessage) {}

		public ValidatorConfigurationException(string message) : base(message) {}

		public ValidatorConfigurationException(string message, Exception inner) : base(message, inner) {}

		public ValidatorConfigurationException(Exception innerException) : base(baseMessage, innerException) {}

		protected ValidatorConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}