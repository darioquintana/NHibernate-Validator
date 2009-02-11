using System.Reflection;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class BooleanConstraints : BaseConstraints, IBooleanConstraints
	{
		public BooleanConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of IBooleanConstraints

		public void IsTrue()
		{
			AddRuleArg(new AssertTrueAttribute());
		}

		public void IsFalse()
		{
			AddRuleArg(new AssertFalseAttribute());
		}

		#endregion
	}
}