using NHibernate.Validator.Engine;
using System;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IInstanceConstraints
	{
		IChainableConstraint<IInstanceConstraints> Using<T>(T attribute) where T : Attribute, IRuleArgs;
	}
}