namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IDateTimeConstraints
	{
		IRuleArgsOptions IsInThePast();
		IRuleArgsOptions IsInTheFuture();
	}
}