using System;
using System.Net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Must be a valid IP address.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class IPAddressAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private string message = "{validator.ipaddress}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			if (value is IPAddress)
			{
				return true;
			}

			string ip = value.ToString();
			IPAddress ipAddress;
			if (IPAddress.TryParse(ip, out ipAddress))
			{
				return true;
			}

			return false;
		}

		#endregion

	}
}