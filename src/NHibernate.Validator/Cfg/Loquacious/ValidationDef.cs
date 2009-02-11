using System;
using System.Linq.Expressions;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class ValidationDef<T>: IValidationDefinition<T> where T : class
	{
		private readonly OpenClassMapping<T> classMap = new OpenClassMapping<T>();

		protected OpenClassMapping<T> ClassMap
		{
			get { return classMap; }
		}

		#region Implementation of IValidationDefinition<T>

		public IDateTimeConstraints Define(Expression<Func<T, DateTime>> property)
		{
			throw new System.NotImplementedException();
		}

		public IDateTimeConstraints Define(Expression<Func<T, DateTime?>> property)
		{
			throw new System.NotImplementedException();
		}

		public IBooleanConstraints Define(Expression<Func<T, bool>> property)
		{
			throw new System.NotImplementedException();
		}

		public IBooleanConstraints Define(Expression<Func<T, bool?>> property)
		{
			throw new System.NotImplementedException();
		}

		public IClassMapping GetMapping()
		{
			return classMap;
		}

		#endregion
	}
}