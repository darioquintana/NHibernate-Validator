using System;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidAttributeNameException : ValidatorConfigurationException
	{
		private readonly string attributeName;
		private readonly System.Type clazz;

		public InvalidAttributeNameException(string attributeName, System.Type clazz)
			: base(
				string.Format(
					@"Attribute '{0}' was not found for the class {1}; 
Cause:
- typo
- Wrong namespace (thye attribute must stay in the same namespace of the related class)",
					attributeName, clazz.FullName))
		{
			this.attributeName = attributeName;
			this.clazz = clazz;
		}

		public string AttributeName
		{
			get { return attributeName; }
		}

		public System.Type Clazz
		{
			get { return clazz; }
		}
	}
}