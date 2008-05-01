using System.Net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class IPAddressValidator : IValidator
	{
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
	}
}