using System.Net;

namespace NHibernate.Validator.Tests.CustomValidator
{
	public class IPSubnetValidator : Validator<IPSubnetAttribute>
	{
		private string subnetPrefix;

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
					return subnetPrefix.Equals(ip.Substring(0, subnetPrefix.Length));
				}
			}

			return false;
		}

		public override void Initialize(IPSubnetAttribute parameters)
		{
			subnetPrefix = parameters.SubnetPrefix;
		}
	}
}
