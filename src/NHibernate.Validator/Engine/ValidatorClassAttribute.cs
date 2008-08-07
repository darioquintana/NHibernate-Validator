using System;

namespace NHibernate.Validator.Engine
{
	public class ValidatorClassAttribute : Attribute
	{
		private readonly System.Type value;
		
		/// <summary>
		/// In order to point to a validator in other assembly.
		/// <code>
		/// <example> 
		/// </example>
		/// </code>
		/// </summary>
		/// <param name="fullAssemblyQualifyName"></param>
		public ValidatorClassAttribute(string fullAssemblyQualifyName)
		{
			value = System.Type.GetType(fullAssemblyQualifyName, true);
		}

		public ValidatorClassAttribute(System.Type value)
		{
			this.value = value;
		}

		public System.Type Value
		{
			get { return value; }
		}
	}
}