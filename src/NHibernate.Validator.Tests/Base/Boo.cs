using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class Boo
	{
		[NotNullNotEmpty]
		public string field;
	}

	public class BooDef: ValidationDef<Boo>
	{
		public BooDef()
		{
			Define(x => x.field).NotNullableAndNotEmpty();
		}
	}
}