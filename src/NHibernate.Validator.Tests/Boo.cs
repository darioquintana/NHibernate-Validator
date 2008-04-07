using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests
{
	public class Boo
	{
		[NotNullOrEmpty]
		public string field;
	}
}
