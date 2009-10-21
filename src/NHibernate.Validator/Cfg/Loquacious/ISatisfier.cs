using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface ISatisfier<TEntity, TConstraints>
		where TConstraints : class
	{
		IChainableConstraint<TConstraints> Satisfy(Func<TEntity, IConstraintValidatorContext, bool> isValidDelegate);
		IChainableConstraint<TConstraints> Satisfy(Func<TEntity, bool> isValidDelegate);
	}
}