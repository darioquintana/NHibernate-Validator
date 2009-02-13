using System;
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