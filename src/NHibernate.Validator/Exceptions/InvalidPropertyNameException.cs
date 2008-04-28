using System;

namespace NHibernate.Validator.Exceptions
{
	[Serializable]
	public class InvalidPropertyNameException : ValidatorConfigurationException
	{
		private readonly string propertyName;
		private readonly System.Type clazz;

		public InvalidPropertyNameException(string propertyName, System.Type clazz)
			:base(string.Format("Property or field \"{0}\" was not found in the class: \"{1}\" ", propertyName, clazz.FullName))
		{
			this.propertyName = propertyName;
			this.clazz = clazz;
		}

		public string PropertyName
		{
			get { return propertyName; }
		}

		public System.Type Clazz
		{
			get { return clazz; }
		}
	}
}
