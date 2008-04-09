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
			
			if (value is string)
			{
				IPAddress ipAddress;
				string ip = value.ToString();
				if (IPAddress.TryParse(ip, out ipAddress))
				{
					return true;
				}
			}

			return false;
		}
	}
}
