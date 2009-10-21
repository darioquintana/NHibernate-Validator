using System;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious.Impl;
using NHibernate.Validator.Mappings;
using System.Collections.Generic;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class ValidationDef<T> : IValidationDefinition<T>, IConstraintAggregator, IMappingSource where T : class
	{
		private readonly OpenClassMapping<T> classMap = new OpenClassMapping<T>();

		#region Implementation of IValidationDefinition<T>

		public IInstanceConstraints<T> ValidateInstance
		{
			get { return new InstanceConstraints<T>(this); }
		}

		public IIntegerConstraints<short> Define(Expression<Func<T, short>> property)
		{
			return new IntegerConstraints<short>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<short?> Define(Expression<Func<T, short?>> property)
		{
			return new IntegerConstraints<short?>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<int> Define(Expression<Func<T, int>> property)
		{
			return new IntegerConstraints<int>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<int?> Define(Expression<Func<T, int?>> property)
		{
			return new IntegerConstraints<int?>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<long> Define(Expression<Func<T, long>> property)
		{
			return new IntegerConstraints<long>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<long?> Define(Expression<Func<T, long?>> property)
		{
			return new IntegerConstraints<long?>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<ushort> Define(Expression<Func<T, ushort>> property)
		{
			return new IntegerConstraints<ushort>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<ushort?> Define(Expression<Func<T, ushort?>> property)
		{
			return new IntegerConstraints<ushort?>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<uint> Define(Expression<Func<T, uint>> property)
		{
			return new IntegerConstraints<uint>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<uint?> Define(Expression<Func<T, uint?>> property)
		{
			return new IntegerConstraints<uint?>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<ulong> Define(Expression<Func<T, ulong>> property)
		{
			return new IntegerConstraints<ulong>(this, DecodeMemberAccessExpression(property));
		}

		public IIntegerConstraints<ulong?> Define(Expression<Func<T, ulong?>> property)
		{
			return new IntegerConstraints<ulong?>(this, DecodeMemberAccessExpression(property));
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

		public ICollectionConstraints Define<Te>(Expression<Func<T, ICollection<Te>>> property)
		{
			return new CollectionConstraints(this, DecodeMemberAccessExpression(property));
		}

		public IRelationshipConstraints Define(Expression<Func<T, object>> property)
		{
			return new RelationshipConstraints(this, DecodeMemberAccessExpression(property));
		}

		public void Add(MemberInfo member, Attribute ruleArgs)
		{
			classMap.AddMemberConstraint(member, ruleArgs);
		}

		public void AddClassConstraint(Attribute ruleArgs)
		{
			classMap.AddEntityValidator(ruleArgs);
		}

		#endregion

		private static MemberInfo DecodeMemberAccessExpression<TResult>(Expression<Func<T, TResult>> expression)
		{
			return TypeUtils.DecodeMemberAccessExpression(expression);
		}

		#region IMappingSource Members

		IClassMapping IMappingSource.GetMapping()
		{
			return classMap;
		}

		#endregion
	}
}