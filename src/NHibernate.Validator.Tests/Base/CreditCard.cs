using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.Base
{
	public class CreditCard
	{
		[CreditCardNumber]
		public string number;

		[EAN]
		public string ean;
	}
}
