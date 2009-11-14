using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class ChainableConstraintBase<TConstraints> : IBasicChainableConstraint<TConstraints> where TConstraints : class
	{
		public ChainableConstraintBase(TConstraints parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			Parent = parent;
		}

		protected TConstraints Parent { get; private set; }

		public TConstraints And
		{
			get { return Parent; }
		}
	}

	public class ChainableConstraint<TConstraints> : ChainableConstraintBase<TConstraints>,
	                                                 IChainableConstraint<TConstraints> where TConstraints : class
	{
		public ChainableConstraint(TConstraints parent, IRuleArgs constraintAttribute) : base(parent)
		{
			ConstraintAttribute = constraintAttribute;
		}

		protected IRuleArgs ConstraintAttribute { get; private set; }

		#region IChainableConstraint<TConstraints> Members

		public IChainableConstraint<TConstraints> WithMessage(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			ConstraintAttribute.Message = message;
			return this;
		}

		public IChainableConstraint<TConstraints> WithTags(params object[] tags)
		{
			if (tags == null)
			{
				return this;
			}
			var tagableRule = ConstraintAttribute as ITagableRule;
			if (tagableRule == null)
			{
				throw new ValidatorConfigurationException(string.Format("The constraint {0} does not supports tags.", ConstraintAttribute.GetType()));
			}
			Array.ForEach(tags, tag => tagableRule.TagCollection.Add(tag));
			return this;
		}

		#endregion
	}
}