using System;
using System.Collections.Generic;
using NHibernate.Validator.Engine;
using System.Reflection;

namespace NHibernate.Validator.Cfg.Loquacious
{
	[CLSCompliant(false)]
	public interface IFluentConfiguration
	{
		INhIntegration IntegrateWithNHibernate { get; }
		IFluentConfiguration SetMessageInterpolator<T>() where T : IMessageInterpolator;
		IFluentConfiguration SetDefaultValidatorMode(ValidatorMode mode);
		IFluentConfiguration SetConstraintValidatorFactory<T>() where T : IConstraintValidatorFactory;
		IFluentConfiguration SetCustomResourceManager(string resourceBaseName, Assembly assembly);
		IFluentConfiguration AddEntityTypeInspector<T>() where T : IEntityTypeInspector;

		IFluentConfiguration Register<TDef, TEntity>() where TDef : IValidationDefinition<TEntity>, IMappingSource, new()
			where TEntity : class;

		IFluentConfiguration Register<TEntity>(IValidationDefinition<TEntity> validationDefinition)
			where TEntity : class;

		IFluentConfiguration Register(IEnumerable<System.Type> definitions);
	}
}