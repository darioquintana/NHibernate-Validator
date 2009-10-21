using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface IValidationDefinition<T> where T : class
	{
		IInstanceConstraints<T> ValidateInstance { get; }

		#region Integer Constraints

		IIntegerConstraints<short> Define(Expression<Func<T, short>> property);
		IIntegerConstraints<short?> Define(Expression<Func<T, short?>> property);
		IIntegerConstraints<int> Define(Expression<Func<T, int>> property);
		IIntegerConstraints<int?> Define(Expression<Func<T, int?>> property);
		IIntegerConstraints<long> Define(Expression<Func<T, long>> property);
		IIntegerConstraints<long?> Define(Expression<Func<T, long?>> property);
		IIntegerConstraints<ushort> Define(Expression<Func<T, ushort>> property);
		IIntegerConstraints<ushort?> Define(Expression<Func<T, ushort?>> property);
		IIntegerConstraints<uint> Define(Expression<Func<T, uint>> property);
		IIntegerConstraints<uint?> Define(Expression<Func<T, uint?>> property);
		IIntegerConstraints<ulong> Define(Expression<Func<T, ulong>> property);
		IIntegerConstraints<ulong?> Define(Expression<Func<T, ulong?>> property);

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

		ICollectionConstraints Define<Te>(Expression<Func<T, ICollection<Te>>> property);
		
		IStringConstraints Define(Expression<Func<T, string>> property);

		IRelationshipConstraints Define(Expression<Func<T, object>> property);
	}
}