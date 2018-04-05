using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	class EnumConstraints : BaseConstraints<IEnumConstraints>, IEnumConstraints 
	{
		public EnumConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IEnumConstraints

		public IChainableConstraint<IEnumConstraints> NotNullable()
		{
			return AddWithConstraintsChain(new NotNullAttribute());
		}

		public IChainableConstraint<IEnumConstraints> Enum()
		{
			return AddWithConstraintsChain(new EnumAttribute());
		}

		#endregion
	}
}
