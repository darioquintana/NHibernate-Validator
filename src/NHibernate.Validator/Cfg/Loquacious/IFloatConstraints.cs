namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IFloatConstraints
	{
		IRuleArgsOptions Digits(int integerDigits);
		IRuleArgsOptions Digits(int integerDigits, int fractionalDigits);
		IRuleArgsOptions LessThanOrEqualTo(long maxValue);
		IRuleArgsOptions LessThanOrEqualTo(decimal maxValue);
		IRuleArgsOptions GreaterThanOrEqualTo(long minValue);
		IRuleArgsOptions GreaterThanOrEqualTo(decimal minValue);
		IRuleArgsOptions IncludedBetween(long minValue, long maxValue);
	}

	public interface IFloatConstraints<T> : IFloatConstraints, ISatisfier<T, IFloatConstraints<T>>
	{

	}
}