namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IEnumConstraints
	{
		IChainableConstraint<IEnumConstraints> NotNullable();

		IChainableConstraint<IEnumConstraints> Enum();
	}
}
