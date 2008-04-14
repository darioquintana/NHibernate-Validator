using System;

namespace NHibernate.Validator.Binding.Controls
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ControlValidableAttribute : Attribute
	{
		private System.Type control;

		public ControlValidableAttribute(System.Type clazz)
		{
			control = clazz;
		}

		public System.Type Control
		{
			get { return control; }
		}
	}
}