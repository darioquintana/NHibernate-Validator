using System;
using System.Runtime.Serialization;
using System.Security;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidPropertyNameException : ValidatorConfigurationException
	{
		public InvalidPropertyNameException(string propertyName, System.Type @class)
			: this(
				string.Format("Property or field \"{0}\" was not found in the class: \"{1}\" ", propertyName, @class.FullName),
				propertyName,
				@class)
		{
		}

		public InvalidPropertyNameException(string message, string propertyName, System.Type @class)
			: base(message)
		{
			PropertyName = propertyName;
			Class = @class;
		}

		protected InvalidPropertyNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			PropertyName = info.GetString(nameof(InvalidPropertyNameException) + "." + nameof(PropertyName));
			var className = info.GetString(nameof(InvalidPropertyNameException) + "." + nameof(Class));
			if (!string.IsNullOrEmpty(className))
				Class = System.Type.GetType(className, true);
		}

		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(
				nameof(InvalidPropertyNameException) + "." + nameof(PropertyName),
				PropertyName);
			info.AddValue(
				nameof(InvalidPropertyNameException) + "." + nameof(Class),
				Class?.AssemblyQualifiedName);
		}

		public string PropertyName { get; }

		public System.Type Class { get; }

		// Since v5.1
		[Obsolete("Please use Class instead.")]
		public System.Type Clazz => Class;
	}
}
