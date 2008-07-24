using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Ar
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(CUITValidator))]
	public class CUITAttribute : Attribute, IRuleArgs
	{
		private string message = "CUIT no valido.";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}