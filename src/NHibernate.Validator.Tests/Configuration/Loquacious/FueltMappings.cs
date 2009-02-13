using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Tests.Base;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	public class AddressValidationDef: ValidationDef<Address>
	{
		public AddressValidationDef()
		{
			Define(x => x.Country)
				.NotNullable().And
				.MaxLength(5);
			Define(x => x.Zip)
				.MaxLength(5).WithMessage("{long}").And
				.MatchWith("[0-9]+");
		}
	}

	public class BooValidationDef : ValidationDef<Boo>
	{
		public BooValidationDef()
		{
			Define(x => x.field).NotNullableAndNotEmpty();
		}
	}
}