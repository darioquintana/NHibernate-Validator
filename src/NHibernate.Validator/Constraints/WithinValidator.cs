using System;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class WithinValidator : IInitializableValidator<WithinAttribute>, IPropertyConstraint
	{
		private double max;
		private double min;

		#region Implementation of IValidator

		public void Initialize(WithinAttribute parameters)
		{
			max = parameters.Max;
			min = parameters.Min;
		}

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				double cvalue = Convert.ToDouble(value);
				return cvalue > min && cvalue < max;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					int i = Convert.ToInt32(value);
					return i > min && i < max;
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

		#endregion

		#region Implementation of IPropertyConstraint

		public void Apply(Property property)
		{
			var col = property.ColumnIterator.OfType<Column>().First();

			var check = new StringBuilder(80);
			if (min != double.MinValue)
			{
				check.Append(col.Name).Append('>').Append(Convert.ToInt64(min));
			}
			if (max != double.MaxValue && min != double.MinValue)
			{
				check.Append(" and ");
			}
			if (max != double.MaxValue)
			{
				check.Append(col.Name).Append('<').Append(Convert.ToInt64(max));
			}
			col.CheckConstraint = check.ToString();
		}

		#endregion
	}
}