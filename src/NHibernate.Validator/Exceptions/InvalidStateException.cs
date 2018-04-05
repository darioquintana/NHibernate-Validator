using System;
using System.Runtime.Serialization;
using System.Security;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidStateException : HibernateException
	{
		public InvalidStateException(string message, Exception inner) : base(message, inner)
		{
		}

		public InvalidStateException(InvalidValue[] invalidValues)
			: this(invalidValues, invalidValues[0].EntityType.Name)
		{
		}

		public InvalidStateException(InvalidValue[] invalidValues, string className)
			: base("validation failed for: " + className)
		{
			InvalidValues = invalidValues;
		}

		protected InvalidStateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InvalidValues = (InvalidValue[]) info.GetValue(
				nameof(InvalidStateException) + "." + nameof(InvalidValues),
				typeof(InvalidValue[]));
		}

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(
				nameof(InvalidStateException) + "." + nameof(InvalidValues),
				InvalidValues);
		}

		public InvalidValue[] InvalidValues { get; }

		// Since 5.1
		[Obsolete("Use InvalidValues property instead.")]
		public InvalidValue[] GetInvalidValues()
		{
			return InvalidValues;
		}
	}
}
