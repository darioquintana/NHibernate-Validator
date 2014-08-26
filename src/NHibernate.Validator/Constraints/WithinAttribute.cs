using System;
using System.Linq;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated elemnt has to be in the appropriate range (excluding both limits).
	/// Apply on numeric values can be converted to double (<see cref="Convert.ToDouble(object)"/>).
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class WithinAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public WithinAttribute() : this(double.MinValue, double.MaxValue, "{validator.within}")
		{
		}

		public WithinAttribute(double min, double max) : this(min, max, "{validator.within}")
		{
		}

		public WithinAttribute(double min, double max, string message)
		{
			this.Min = min;
			this.Max = max;
			this.Message = message;
		}

		public double Min { get; set; }

		public double Max { get; set; }

		#region Implementation of IValidator

		public override bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				double cvalue = Convert.ToDouble(value);
				return cvalue > Min && cvalue < Max;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					int i = Convert.ToInt32(value);
					return i > Min && i < Max;
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
			if (Min != double.MinValue)
			{
				check.Append(col.Name).Append('>').Append(Convert.ToInt64(Min));
			}
			if (Max != double.MaxValue && Min != double.MinValue)
			{
				check.Append(" and ");
			}
			if (Max != double.MaxValue)
			{
				check.Append(col.Name).Append('<').Append(Convert.ToInt64(Max));
			}
			col.CheckConstraint = check.ToString();
		}

		#endregion
	}
}