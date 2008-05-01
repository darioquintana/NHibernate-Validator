using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class MaxValidator : IInitializableValidator<MaxAttribute>, IPropertyConstraint
	{
		private double max;

		#region IInitializableValidator<MaxAttribute> Members

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				return Convert.ToDouble(value) <= max;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					return Convert.ToInt32(value) <= max;
				}
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			catch (OverflowException)
			{
				return false;
			}
		}

		public void Initialize(MaxAttribute parameters)
		{
			max = parameters.Value;
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column col = (Column) ie.Current;
			col.CheckConstraint = col.Name + "<=" + max;
		}

		#endregion
	}
}