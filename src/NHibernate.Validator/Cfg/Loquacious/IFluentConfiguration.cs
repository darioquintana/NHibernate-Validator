using System.Collections.Generic;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IFluentConfiguration
	{
		INhIntegration IntegrateWithNHibernate { get; }
		IFluentConfiguration SetMessageInterpolator<T>() where T : IMessageInterpolator;
		IFluentConfiguration SetDefaultValidatorMode(ValidatorMode mode);

		IFluentConfiguration Register<TDef, TEntity>() where TDef : IValidationDefinition<TEntity>, IMappingSource, new()
			where TEntity : class;

		IFluentConfiguration Register(IEnumerable<System.Type> definitions);
	}
}