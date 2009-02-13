using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class CollectionConstraints : BaseConstraints<ICollectionConstraints>, ICollectionConstraints
	{
		public CollectionConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of ICollectionConstraints

		public IChainableConstraint<ICollectionConstraints> NotNullable()
		{
			return AddWithConstraintsChain(new NotNullAttribute());
		}

		public IChainableConstraint<ICollectionConstraints> NotEmpty()
		{
			return AddWithConstraintsChain(new NotEmptyAttribute());
		}

		public IChainableConstraint<ICollectionConstraints> NotNullableAndNotEmpty()
		{
			return AddWithConstraintsChain(new NotNullNotEmptyAttribute());
		}

		public IRuleArgsOptions MaxSize(int maxSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute { Max = maxSize});
		}

		public IRuleArgsOptions MinSize(int minSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute { Min = minSize });
		}

		public IRuleArgsOptions SizeBetween(int minSize, int maxSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute { Min = minSize, Max = minSize });
		}

		public IBasicChainableConstraint<ICollectionConstraints> HasValidElements()
		{
			AddRuleArg(new ValidAttribute());

			return new ChainableConstraintBase<ICollectionConstraints>(this);
		}

		#endregion
	}
}