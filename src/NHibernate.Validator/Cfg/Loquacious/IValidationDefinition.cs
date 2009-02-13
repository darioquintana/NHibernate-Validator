using System;
using System.Collections;
using System.Linq.Expressions;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IValidationDefinition<T> where T : class
	{
		#region Integer Constraints

		IIntegerConstraints Define(Expression<Func<T, short>> property);
		IIntegerConstraints Define(Expression<Func<T, short?>> property);
		IIntegerConstraints Define(Expression<Func<T, int>> property);
		IIntegerConstraints Define(Expression<Func<T, int?>> property);
		IIntegerConstraints Define(Expression<Func<T, long>> property);
		IIntegerConstraints Define(Expression<Func<T, long?>> property);
		IIntegerConstraints Define(Expression<Func<T, ushort>> property);
		IIntegerConstraints Define(Expression<Func<T, ushort?>> property);
		IIntegerConstraints Define(Expression<Func<T, uint>> property);
		IIntegerConstraints Define(Expression<Func<T, uint?>> property);
		IIntegerConstraints Define(Expression<Func<T, ulong>> property);
		IIntegerConstraints Define(Expression<Func<T, ulong?>> property);

		#endregion

		#region floating point constraints

		IFloatConstraints Define(Expression<Func<T, float>> property);
		IFloatConstraints Define(Expression<Func<T, float?>> property);
		IFloatConstraints Define(Expression<Func<T, double>> property);
		IFloatConstraints Define(Expression<Func<T, double?>> property);
		IFloatConstraints Define(Expression<Func<T, decimal>> property);
		IFloatConstraints Define(Expression<Func<T, decimal?>> property);

		#endregion

		#region DateTime constraints

		IDateTimeConstraints Define(Expression<Func<T, DateTime>> property);
		IDateTimeConstraints Define(Expression<Func<T, DateTime?>> property);

		#endregion

		#region DateTime constraints

		IBooleanConstraints Define(Expression<Func<T, bool>> property);
		IBooleanConstraints Define(Expression<Func<T, bool?>> property);

		#endregion

		IStringConstraints Define(Expression<Func<T, string>> property);

		ICollectionConstraints Define(Expression<Func<T, ICollection>> property);

		IReletionshipConstraints Define(Expression<Func<T, object>> property);
	}
}