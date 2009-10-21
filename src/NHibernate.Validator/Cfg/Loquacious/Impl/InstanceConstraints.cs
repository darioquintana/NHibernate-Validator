using System;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class InstanceConstraints<TEntity> : IInstanceConstraints<TEntity> where TEntity : class
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

		public IChainableConstraint<IInstanceConstraints<TEntity>> Using<T>(T attribute) where T : Attribute, IRuleArgs
		{
			parent.AddClassConstraint(attribute);
			return new ChainableConstraint<IInstanceConstraints<TEntity>>(this, attribute);
		}

		public IChainableConstraint<IInstanceConstraints<TEntity>> By(Func<TEntity, IConstraintValidatorContext, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedConstraint<TEntity>(isValidDelegate));
			parent.AddClassConstraint(attribute);
			return new ChainableConstraint<IInstanceConstraints<TEntity>>(this, attribute);
		}

		#endregion
	}
}