namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IIntegerConstraints
	{
		IRuleArgsOptions Digits(int maxIntegerDigits);
		IRuleArgsOptions Digits(int maxIntegerDigits, int maxFractionalDigits);
		IRuleArgsOptions LessThanOrEqualTo(long maxValue);
		IRuleArgsOptions GreaterThanOrEqualTo(long minValue);
		IRuleArgsOptions IncludedBetween(long minValue, long maxValue);
		IRuleArgsOptions Whitih(long minValue, long maxValue);
		IRuleArgsOptions IsEAN();
	}

	public interface IIntegerConstraints<T> : IIntegerConstraints, ISatisfier<T, IIntegerConstraints<T>>
	{
		
	}
}