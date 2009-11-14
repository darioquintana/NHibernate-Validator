namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IRuleArgsOptions
	{
		void WithMessage(string message);
		void WithTags(params object[] tags);
	}
}