using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ConstraintFactory.Model
{
	[My]
	public class Foo
	{
		[CreditCardNumber]
		public string CreditCard { get; set; }

		[My]
		public string Description { get; set; }
	}
}