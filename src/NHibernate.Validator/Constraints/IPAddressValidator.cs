using System;
using System.Net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class IPAddressValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value)
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