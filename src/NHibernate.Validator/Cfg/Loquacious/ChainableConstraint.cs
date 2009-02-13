using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class ChainableConstraint<TConstraints> : IChainableConstraint<TConstraints> where TConstraints : class
	{
		public ChainableConstraint(TConstraints parent, IRuleArgs constraintAttribute)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			Parent = parent;
			ConstraintAttribute = constraintAttribute;
		}

		protected TConstraints Parent { get; private set; }
		protected IRuleArgs ConstraintAttribute { get; private set; }

		#region IChainableConstraint<TConstraints> Members

		public TConstraints And
		{
			get { return Parent; }
		}

		public IChainableConstraint<TConstraints> WithMessage(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			ConstraintAttribute.Message = message;
			return this;
		}

		#endregion
	}
}