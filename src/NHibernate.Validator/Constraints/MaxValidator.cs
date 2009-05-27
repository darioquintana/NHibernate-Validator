using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class MaxValidator : IInitializableValidator<MaxAttribute>, IPropertyConstraint
	{
		private double limit;

		#region IInitializableValidator<MaxAttribute> Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				return Convert.ToDouble(value) <= limit;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					return Convert.ToInt32(value) <= limit;
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
			limit = parameters.Value;
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column) ie.Current;
			col.CheckConstraint = col.Name + "<=" + limit;
		}

		#endregion
	}
}