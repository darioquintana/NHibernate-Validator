using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class ReletionshipConstraints : BaseConstraints<IReletionshipConstraints>, IReletionshipConstraints
	{
		public ReletionshipConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IReletionshipConstraints

		public IChainableConstraint<IReletionshipConstraints> NotNullable()
		{
			return AddWithConstraintsChain(new NotNullAttribute());
		}

		public IBasicChainableConstraint<IReletionshipConstraints> IsValid()
		{
			AddRuleArg(new ValidAttribute());

			return new ChainableConstraintBase<IReletionshipConstraints>(this);
		}

		#endregion
	}
}