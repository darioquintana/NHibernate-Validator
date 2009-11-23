using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class GuidConstraints : BaseConstraints, IGuidConstraints
	{
		public GuidConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IGuidConstraints

		public IRuleArgsOptions NotEmpty()
		{
			return AddWithFinalRuleArgOptions(new NotNullNotEmptyAttribute());
		}

		#endregion
	}
}