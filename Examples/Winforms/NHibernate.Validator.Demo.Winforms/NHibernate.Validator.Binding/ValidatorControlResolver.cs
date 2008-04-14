using System;
using System.Collections.Generic;
using NHibernate.Validator.Binding.Controls;

namespace NHibernate.Validator.Binding
{
	public class ValidatorControlResolver
	{
		private List<System.Type> resolver;

		public ValidatorControlResolver(List<System.Type> resolver)
		{
			this.resolver = resolver;
		}

		public ValidatorControlResolver()
		{
			//Register default resolvers
			resolver = new List<System.Type>();
			resolver.Add(typeof(TextValuable));
			resolver.Add(typeof(DateTimePickerValuable));
		}

		public IControlValuable GetControlValuable(object control)
		{
			foreach(System.Type type in resolver)
			{
				foreach(object o in type.GetCustomAttributes(typeof(ControlValidableAttribute), false))
				{
					ControlValidableAttribute attribute = (ControlValidableAttribute) o;
					if (attribute.Control.IsInstanceOfType(control))
						return (IControlValuable) Activator.CreateInstance(type);
				}
			}
			throw new ArgumentException("Could not find the IControlValuable for this control");
		}

		public void Add(System.Type controlValuable)
		{
			resolver.Add(controlValuable);
		}
	}
}