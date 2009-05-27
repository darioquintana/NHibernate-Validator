using System;
using System.Linq.Expressions;

namespace NHibernate.Validator.Engine
{
	public class ConstraintValidatorContext : IConstraintValidatorContext
	{
		public void DisableDefaultError()
		{
			throw new NotImplementedException();
		}

		public string GetDefaultErrorMessage()
		{
			throw new NotImplementedException();
		}

		public void AddInvalid(string message)
		{
			throw new NotImplementedException();
		}

		public void AddInvalid(string message, string property)
		{
			throw new NotImplementedException();
		}

		public void AddInvalid<TEntity, TProperty>(string message, Expression<Func<TEntity, TProperty>> property)
		{
			throw new NotImplementedException();
		}
	}
}