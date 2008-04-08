using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(DigitsValidator))]
	public class DigitsAttribute : Attribute, IHasMessage
	{
		private string message = "{validator.digits}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}