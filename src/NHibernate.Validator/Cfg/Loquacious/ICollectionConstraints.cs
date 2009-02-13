namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface ICollectionConstraints
	{
		IChainableConstraint<ICollectionConstraints> NotNullable();
		IChainableConstraint<ICollectionConstraints> NotEmpty();
		IRuleArgsOptions MaxSize(int maxSize);
		IRuleArgsOptions MinSize(int minSize);
		IRuleArgsOptions SizeBetween(int minSize, int maxSize);
	}
}