using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.ConstraintFactory.Model
{
	public class FooDef : ValidationDef<Foo>
	{
		public FooDef()
		{
			ValidateInstance.Using(new MyAttribute());
			Define(x => x.CreditCard).IsCreditCardNumber();
			Define(x => x.Description).NotNullableAndNotEmpty().And.LengthBetween(10, 300);
		}
	}
}