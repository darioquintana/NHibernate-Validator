using System.Net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class IPAddressValidator : Validator<IPAddressAttribute>
	{
		public override bool IsValid(object value)
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

		public override void Initialize(IPAddressAttribute parameters)
		{
		}
	}
}
