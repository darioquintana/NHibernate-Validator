using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.IPFixture
{
	public class Computer
	{
		[IPAddress]
		public string IpAddress;
	}
}
