namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IChainableConstraint<TConstraints> where TConstraints : class
	{
		TConstraints And { get; }
		IChainableConstraint<TConstraints> WithMessage(string message);
	}
}