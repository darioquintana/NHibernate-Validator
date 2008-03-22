using System;
using System.Runtime.Serialization;

namespace NHibernate.Validator
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