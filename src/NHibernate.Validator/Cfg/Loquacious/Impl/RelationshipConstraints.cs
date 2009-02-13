using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class RelationshipConstraints : BaseConstraints<IRelationshipConstraints>, IRelationshipConstraints
	{
		public RelationshipConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IReletionshipConstraints

		public IChainableConstraint<IRelationshipConstraints> NotNullable()
		{
			return AddWithConstraintsChain(new NotNullAttribute());
		}

		public IBasicChainableConstraint<IRelationshipConstraints> IsValid()
		{
			AddRuleArg(new ValidAttribute());

			return new ChainableConstraintBase<IRelationshipConstraints>(this);
		}

		#endregion
	}
}