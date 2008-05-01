using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(CreditCardNumberValidator))]
	public class CreditCardNumberAttribute : Attribute, IRuleArgs
	{
		#region IRuleArgs Members

		private string message = "{validator.creditCardNumber}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}
