using System;
using System.Runtime.Serialization;


namespace NHibernate.Validator.Exceptions
{
	/// <summary>
	/// Indicates failure of an assertion: a possible bug in NHibernate.Validator
	/// </summary>
	// 6.0 TODO: cease deriving from ApplicationException
	[Serializable]
	public class AssertionFailureException : ApplicationException
	{
		private const string defMessage = "An AssertionFailure occured - this may indicate a bug in NHibernate.Validator";

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertionFailureException"/> class.
		/// </summary>
		public AssertionFailureException()
			: base(string.Empty)
		{
#if NETFX
			LoggerProvider.LoggerFor(typeof(AssertionFailureException)).Error(defMessage);
#else
			NHibernateLogger.For(typeof(AssertionFailureException)).Error(defMessage);
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertionFailureException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public AssertionFailureException(string message)
			: base(message)
		{
#if NETFX
			LoggerProvider.LoggerFor(typeof(AssertionFailureException)).Error(defMessage);
#else
			NHibernateLogger.For(typeof(AssertionFailureException)).Error(defMessage);
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertionFailureException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception. If the innerException parameter 
		/// is not a null reference, the current exception is raised in a catch block that handles 
		/// the inner exception.
		/// </param>
		public AssertionFailureException(string message, Exception innerException)
			: base(message, innerException)
		{
#if NETFX
			LoggerProvider.LoggerFor(typeof (AssertionFailureException)).Error(defMessage);
#else
			NHibernateLogger.For(typeof(AssertionFailureException)).Error(defMessage);
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertionFailureException"/> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo"/> that holds the serialized object 
		/// data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <c>StreamingContext</c> that contains contextual information about the source or destination.
		/// </param>
		protected AssertionFailureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
