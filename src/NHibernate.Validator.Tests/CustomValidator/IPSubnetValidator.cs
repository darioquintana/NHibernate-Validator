using System.Net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.CustomValidator
{
	public class IPSubnetValidator : IInitializableValidator<IPSubnetAttribute>
	{
		private string subnetPrefix;

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
					return subnetPrefix.Equals(ip.Substring(0, subnetPrefix.Length));
				}
			}

			return false;
		}

		public void Initialize(IPSubnetAttribute parameters)
		{
			subnetPrefix = parameters.SubnetPrefix;
		}
	}
}
