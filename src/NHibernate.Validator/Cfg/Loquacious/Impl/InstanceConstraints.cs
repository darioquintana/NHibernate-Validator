using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class InstanceConstraints : IInstanceConstraints
	{
		private readonly IConstraintAggregator parent;

		public InstanceConstraints(IConstraintAggregator parent)
		{
			this.parent = parent;
		}

		protected IConstraintAggregator Parent
		{
			get { return parent; }
		}

		#region Implementation of IInstanceConstraints

		public IChainableConstraint<IInstanceConstraints> Using<T>(T attribute) where T : Attribute, IRuleArgs
		{
			parent.AddClassConstraint(attribute);
			return new ChainableConstraint<IInstanceConstraints>(this, attribute);
		}

		#endregion
	}
}