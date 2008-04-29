using System;

namespace NHibernate.Validator.Engine
{
	public class ValidatorClassAttribute : Attribute
	{
		private readonly System.Type value;

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