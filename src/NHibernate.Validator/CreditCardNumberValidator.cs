using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class CreditCardNumberValidator : AbstractLuhnValidator, IValidator
	{
		public override int multiplicator()
		{
			return 2;
		}
	}
}
