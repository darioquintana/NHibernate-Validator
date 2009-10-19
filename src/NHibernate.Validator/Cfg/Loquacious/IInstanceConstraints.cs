using NHibernate.Validator.Engine;
using System;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IInstanceConstraints<TEntity> where TEntity: class
	{
		IChainableConstraint<IInstanceConstraints<TEntity>> Using<T>(T attribute) where T : Attribute, IRuleArgs;
		IChainableConstraint<IInstanceConstraints<TEntity>> By(Func<TEntity, IConstraintValidatorContext, bool> isValidDelegate);
	}
}