namespace NHibernate.Validator.Tests.Base
{
	public class CreditCard
	{
		[EAN(Message="Invalid EAN number")] public string ean;

		[CreditCardNumber(Message="Invalid credit card number")] public string number;
	}
}