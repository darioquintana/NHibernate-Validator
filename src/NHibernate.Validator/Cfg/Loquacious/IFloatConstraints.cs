namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IFloatConstraints
	{
		IRuleArgsOptions Digits(int maxIntegerDigits);
		IRuleArgsOptions Digits(int maxIntegerDigits, int maxFractionalDigits);
		IRuleArgsOptions LessThanOrEqualTo(long maxValue);
		IRuleArgsOptions LessThanOrEqualTo(decimal maxValue);
		IRuleArgsOptions GreaterThanOrEqualTo(long minValue);
		IRuleArgsOptions GreaterThanOrEqualTo(decimal minValue);
		IRuleArgsOptions IncludedBetween(long minValue, long maxValue);
		IRuleArgsOptions Whitih(double minValue, double maxValue);
	}

	public interface IFloatConstraints<T> : IFloatConstraints, ISatisfier<T, IFloatConstraints<T>>
	{

	}
}