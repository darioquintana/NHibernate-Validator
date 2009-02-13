namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IRelationshipConstraints
	{
		IChainableConstraint<IRelationshipConstraints> NotNullable();
		IBasicChainableConstraint<IRelationshipConstraints> IsValid();
	}
}