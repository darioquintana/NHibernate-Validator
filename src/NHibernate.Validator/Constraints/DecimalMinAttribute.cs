using System;
using System.Collections;
using System.Globalization;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Min restriction on a numeric annotated element (or the string representation of a numeric)
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DecimalMinAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public DecimalMinAttribute() : this(0D, "{validator.min}")
		{
		}

		public DecimalMinAttribute(double min) : this(min, "{validator.min}")
		{
		}

		public DecimalMinAttribute(decimal min) : this(min, "{validator.min}")
		{
		}

		public DecimalMinAttribute(decimal min, string message)
		{
			Value = min;
			this.ErrorMessage = message;
		}

		public DecimalMinAttribute(double min, string message)
		{
			Value = (decimal) min;
			this.ErrorMessage = message;
		}

		public decimal Value { get; set; }

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}
			try
			{
				return Convert.ToDecimal(value) >= Value;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					return Convert.ToInt32(value) >= Value;
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
			var col = (Column)ie.Current;
            col.CheckConstraint = col.Name + ">=" + Convert.ToString(Value, CultureInfo.InvariantCulture);
		}
	}
}
