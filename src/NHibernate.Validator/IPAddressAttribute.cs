using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(IPAddressValidator))]
	public class IPAddressAttribute : Attribute
	{
		private string message = "{validator.ipAddress}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

	}
}
