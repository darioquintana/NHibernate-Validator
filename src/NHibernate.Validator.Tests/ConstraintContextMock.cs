using System;
using System.Linq.Expressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests
{
	public class ConstraintContextMock : IConstraintValidatorContext
	{
		#region IConstraintValidatorContext Members

		public void DisableDefaultError()
		{
			throw new NotImplementedException();
		}

		public string DefaultErrorMessage
		{
			get { throw new NotImplementedException(); }
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

		#endregion
	}
}