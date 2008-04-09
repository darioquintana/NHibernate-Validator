using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class MaxValidator : IInitializableValidator<MaxAttribute>, IPropertyConstraint
	{
		private long max;

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			if (value is string)
			{
				try
				{
					return Convert.ToDecimal(value) <= Convert.ToDecimal(max);
				}
				catch (FormatException)
				{
					return false;
				}
			}
			else if (value is decimal)
			{
				return Convert.ToDecimal(value) <= Convert.ToDecimal(max);
			}
			else if (value is Int32)
			{
				return Convert.ToInt32(value) <= Convert.ToInt32(max);
			}
			else if (value is Int64)
			{
				return Convert.ToInt64(value) <= Convert.ToInt64(max);
			}
			else if (value is long)
			{
				return (long)value <= max;
			}

			return false;
		}

		public void Initialize(MaxAttribute parameters)
		{
			max = parameters.Value;
		}

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column col = (Column)ie.Current;
			col.CheckConstraint = col.Name + "<=" + max;
		}
	}
}