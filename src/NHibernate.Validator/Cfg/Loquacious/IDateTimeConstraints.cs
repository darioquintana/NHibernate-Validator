namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IDateTimeConstraints
	{
		void IsInThePast();
		void IsInTheFuture();
	}
}