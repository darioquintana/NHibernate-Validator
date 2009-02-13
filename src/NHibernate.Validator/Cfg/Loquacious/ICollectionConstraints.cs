namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface ICollectionConstraints
	{
		IChainableConstraint<ICollectionConstraints> NotNullable();
		IChainableConstraint<ICollectionConstraints> NotEmpty();
		IChainableConstraint<ICollectionConstraints> NotNullableAndNotEmpty();
		IRuleArgsOptions MaxSize(int maxSize);
		IRuleArgsOptions MinSize(int minSize);
		IRuleArgsOptions SizeBetween(int minSize, int maxSize);
		IBasicChainableConstraint<ICollectionConstraints> HasValidElements();
	}
}