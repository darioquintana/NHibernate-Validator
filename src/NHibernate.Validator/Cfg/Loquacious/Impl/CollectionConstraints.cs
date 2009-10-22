using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class CollectionConstraints<TElement> : BaseConstraints<ICollectionConstraints<TElement>>,
	                                               ICollectionConstraints<TElement>
	{
		public CollectionConstraints(IConstraintAggregator parent, MemberInfo member) : base(parent, member) {}

		#region Implementation of ICollectionConstraints

		public IChainableConstraint<ICollectionConstraints> NotNullable()
		{
			return AddWithBaseConstraintsChain(new NotNullAttribute());
		}

		public IChainableConstraint<ICollectionConstraints> NotEmpty()
		{
			return AddWithBaseConstraintsChain(new NotEmptyAttribute());
		}

		public IChainableConstraint<ICollectionConstraints> NotNullableAndNotEmpty()
		{
			return AddWithBaseConstraintsChain(new NotNullNotEmptyAttribute());
		}

		public IRuleArgsOptions MaxSize(int maxSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute {Max = maxSize});
		}

		public IRuleArgsOptions MinSize(int minSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute {Min = minSize});
		}

		public IRuleArgsOptions SizeBetween(int minSize, int maxSize)
		{
			return AddWithFinalRuleArgOptions(new SizeAttribute {Min = minSize, Max = maxSize});
		}

		public IBasicChainableConstraint<ICollectionConstraints> HasValidElements()
		{
			AddRuleArg(new ValidAttribute());

			return new ChainableConstraintBase<ICollectionConstraints>(this);
		}

		#endregion

		public IChainableConstraint<ICollectionConstraints> AddWithBaseConstraintsChain<TRuleArg>(TRuleArg ruleArgs)
			where TRuleArg : Attribute, IRuleArgs
		{
			AddRuleArg(ruleArgs);
			return new ChainableConstraint<ICollectionConstraints>(this, ruleArgs);
		}

		#region Implementation of ISatisfier<IEnumerable<TElement>,ICollectionConstraints<TElement>>

		public IChainableConstraint<ICollectionConstraints<TElement>> Satisfy(Func<IEnumerable<TElement>, IConstraintValidatorContext, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedConstraint<IEnumerable<TElement>>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		public IChainableConstraint<ICollectionConstraints<TElement>> Satisfy(Func<IEnumerable<TElement>, bool> isValidDelegate)
		{
			var attribute = new DelegatedValidatorAttribute(new DelegatedSimpleConstraint<IEnumerable<TElement>>(isValidDelegate));
			return AddWithConstraintsChain(attribute);
		}

		#endregion
	}
}