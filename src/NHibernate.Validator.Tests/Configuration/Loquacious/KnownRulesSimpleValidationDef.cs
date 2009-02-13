using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	public class KnownRulesSimpleValidationDef : ValidationDef<KnownRules>
	{
		public KnownRulesSimpleValidationDef()
		{
			Define(x => x.DtProp).IsInThePast();
		}
	}
}