using System;
using System.Linq.Expressions;

namespace NHibernate.Validator.Engine
{
	public interface IConstraintValidatorContext
	{
		void DisableDefaultError();

		string DefaultErrorMessage { get; }

		void AddInvalid(string message);

		void AddInvalid(string message, string property);

		void AddInvalid<TEntity, TProperty>(string message, Expression<Func<TEntity, TProperty>> property);
	}
}