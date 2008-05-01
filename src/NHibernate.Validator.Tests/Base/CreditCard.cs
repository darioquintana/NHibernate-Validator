using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.Base
{
	public class CreditCard
	{
		[CreditCardNumber(Message="Invalid credit card number")]
		public string number;

		[EAN(Message="Invalid EAN number")]
		public string ean;
	}
}
