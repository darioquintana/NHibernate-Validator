using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class RangeValidator : IInitializableValidator<RangeAttribute>, IPropertyConstraint
	{
		private double min;
		private double max;

		public void Initialize(RangeAttribute parameters)
		{
			max = parameters.Max;
			min = parameters.Min;
		}

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				double cvalue = Convert.ToDouble(value);
				return cvalue >= min && cvalue <= max;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					int i = Convert.ToInt32(value);
					return i >= min && i <= max;
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

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column col = (Column)ie.Current;

			String check = "";
			if (min != long.MinValue) check += col.Name + ">=" + min;
			if (max != long.MaxValue && min != long.MinValue) check += " and ";
			if (max != long.MaxValue) check += col.Name + "<=" + max;
			col.CheckConstraint = check;
		}
	}
}