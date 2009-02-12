using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Specific.It
{
	public static class ItLoquaciousExtensions
	{
		public static IRuleArgsOptions IsCodiceFiscale(this IStringConstraints definition)
		{
			return ((IConstraints)definition).AddWithFinalRuleArgOptions(new CodiceFiscaleAttribute());
		}

		public static IRuleArgsOptions IsPartitaIva(this IStringConstraints definition)
		{
			return ((IConstraints)definition).AddWithFinalRuleArgOptions(new PartitaIvaAttribute());
		}
	}
}