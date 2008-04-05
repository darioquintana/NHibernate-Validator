using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(IPSubnetValidator))]
	public class IPSubnetAttribute : Attribute
	{
		private string subnetPrefix;
		private string message = "{validator.ipSubnet}";

		public string SubnetPrefix
		{
			get { return subnetPrefix; }
			set { subnetPrefix = value; }
		}

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

	}
}
