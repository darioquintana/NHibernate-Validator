using System;

namespace NHibernate.Validator.Engine
{
	public class ValidatorClassAttribute : Attribute
	{
		private System.Type value;

		public ValidatorClassAttribute()
		{
		}

		public ValidatorClassAttribute(System.Type value)
		{
			this.value = value;
		}

		public System.Type Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}
}