namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IIntegerConstraints
	{
		IRuleArgsOptions Digits(int digits);
		IRuleArgsOptions LessThanOrEqualTo(long maxValue);
		IRuleArgsOptions GreaterThanOrEqualTo(long minValue);
		IRuleArgsOptions IncludedBetween(long minValue, long maxValue);
		IRuleArgsOptions IsEAN();
	}
}