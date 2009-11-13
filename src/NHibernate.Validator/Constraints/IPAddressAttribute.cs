using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Must be a valid IP address.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (IPAddressValidator))]
	public class IPAddressAttribute : EmbeddedRuleArgsAttribute, IRuleArgs
	{
		private string message = "{validator.ipaddress}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}