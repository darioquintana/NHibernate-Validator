using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.ConstraintFactory.Model
{
	public class FooDef : ValidationDef<Foo>
	{
		public FooDef()
		{
			Define(x => x.Name).NotNullableAndNotEmpty();
			Define(x => x.Description).NotNullableAndNotEmpty().And.LengthBetween(10, 300);
		}
	}
}