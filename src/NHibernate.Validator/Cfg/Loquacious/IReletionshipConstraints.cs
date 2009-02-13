namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IReletionshipConstraints
	{
		IChainableConstraint<IReletionshipConstraints> NotNullable();
		IBasicChainableConstraint<IReletionshipConstraints> IsValid();
	}
}