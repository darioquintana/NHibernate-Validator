using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class CreditCardNumberValidator : AbstractLuhnValidator, IValidator
	{
		public override int Multiplicator
		{
			get { return 2; }
		}
	}
}