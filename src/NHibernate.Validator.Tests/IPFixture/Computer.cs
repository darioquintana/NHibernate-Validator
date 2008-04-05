using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.IPFixture
{
	public class Computer
	{
		[IPAddress]
		public string IpAddress;
	}
}
