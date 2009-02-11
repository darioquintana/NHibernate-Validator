namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IDateTimeConstraints
	{
		IDateTimeConstraints IsInThePast();
		IDateTimeConstraints IsInTheFuture();
	}
}