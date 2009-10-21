namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IDateTimeConstraints
	{
		IRuleArgsOptions IsInThePast();
		IRuleArgsOptions IsInTheFuture();
	}

	public interface IDateTimeConstraints<T> : IDateTimeConstraints, ISatisfier<T, IDateTimeConstraints<T>>
	{

	}
}