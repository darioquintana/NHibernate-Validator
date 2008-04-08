using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (IPAddressValidator))]
	public class IPAddressAttribute : Attribute, IHasMessage
	{
		private string message = "{validator.ipAddress}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}