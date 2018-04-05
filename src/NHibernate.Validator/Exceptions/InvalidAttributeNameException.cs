using System;
using System.Runtime.Serialization;
using System.Security;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidAttributeNameException : ValidatorConfigurationException
	{
		public InvalidAttributeNameException(string attributeName, System.Type @class)
			: base(
				string.Format(
					@"Attribute '{0}' was not found for the class {1};
Cause:
- typo
- Wrong namespace (the attribute must stay in the same namespace of the related class)",
					attributeName,
					@class.FullName))
		{
			AttributeName = attributeName;
			Class = @class;
		}

		protected InvalidAttributeNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			AttributeName = info.GetString(nameof(InvalidAttributeNameException) + "." + nameof(AttributeName));
			var className = info.GetString(nameof(InvalidAttributeNameException) + "." + nameof(Class));
			if (!string.IsNullOrEmpty(className))
				Class = System.Type.GetType(className, true);
		}

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(
				nameof(InvalidAttributeNameException) + "." + nameof(AttributeName),
				AttributeName);
			info.AddValue(
				nameof(InvalidAttributeNameException) + "." + nameof(Class),
				Class?.AssemblyQualifiedName);
		}

		public string AttributeName { get; }

		public System.Type Class { get; }
	}
}
