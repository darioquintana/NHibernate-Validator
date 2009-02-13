namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IBasicChainableConstraint<TConstraints> where TConstraints : class
	{
		TConstraints And { get; }
	}

	public interface IChainableConstraint<TConstraints> : IBasicChainableConstraint<TConstraints>
		where TConstraints : class
	{
		IChainableConstraint<TConstraints> WithMessage(string message);
	}
}