using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Must be a valid IP address.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (IPAddressValidator))]
	public class IPAddressAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.ipaddress}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}