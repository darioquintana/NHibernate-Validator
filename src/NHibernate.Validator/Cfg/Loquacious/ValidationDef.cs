using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class ValidationDef<T> : IValidationDefinition<T>, IConstraintAggregator, IMappingSource where T : class
	{
		private readonly OpenClassMapping<T> classMap = new OpenClassMapping<T>();

		#region Implementation of IValidationDefinition<T>

		public IIntegerConstraints Define(Expression<Func<T, short>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, short?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, int>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, int?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, long>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, long?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, ushort>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, ushort?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, uint>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, uint?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, ulong>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints Define(Expression<Func<T, ulong?>> property)
		{
			return new IntegerConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, float>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, float?>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, double>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, double?>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, decimal>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IFloatConstraints Define(Expression<Func<T, decimal?>> property)
		{
			return new FloatConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IDateTimeConstraints Define(Expression<Func<T, DateTime>> property)
		{
			return new DateTimeConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IDateTimeConstraints Define(Expression<Func<T, DateTime?>> property)
		{
			return new DateTimeConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IBooleanConstraints Define(Expression<Func<T, bool>> property)
		{
			return new BooleanConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IBooleanConstraints Define(Expression<Func<T, bool?>> property)
		{
			return new BooleanConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IStringConstraints Define(Expression<Func<T, string>> property)
		{
			return new StringConstraints(this, DecodeMemberAccessExpression(property));
		}

		public ICollectionConstraints Define(Expression<Func<T, ICollection>> property)
		{
			return new CollectionConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IReletionshipConstraints Define(Expression<Func<T, object>> property)
		{
			return new ReletionshipConstraints(this, DecodeMemberAccessExpression(property));
		}

		public void Add(MemberInfo member, Attribute ruleArgs)
		{
			classMap.AddMemberConstraint(member, ruleArgs);
		}

		#endregion

		private static MemberInfo DecodeMemberAccessExpression<TResult>(Expression<Func<T, TResult>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				throw new HibernateValidatorException(
					string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", expression.Body.NodeType));
			}
			return ((MemberExpression)expression.Body).Member;
		}

		#region IMappingSource Members

		IClassMapping IMappingSource.GetMapping()
		{
			return classMap;
		}

		#endregion
	}
}