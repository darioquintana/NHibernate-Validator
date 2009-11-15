using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class CreditCard
	{
		[EAN(Message="Invalid EAN number")]
		public string Ean { get; set; }

		[CreditCardNumber(Message="Invalid credit card number")]
		public string Number { get; set; }
	}
}