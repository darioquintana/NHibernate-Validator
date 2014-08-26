using System;
using System.Collections;
using System.Globalization;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Max restriction on a numeric annotated element
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DecimalMaxAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public DecimalMaxAttribute() : this(0D, "{validator.max}")
		{
		}

		public DecimalMaxAttribute(double max) : this(max, "{validator.max}")
		{
		}

		public DecimalMaxAttribute(decimal max) : this(max, "{validator.max}")
		{
		}

		public DecimalMaxAttribute(decimal max, string message)
		{
			this.Value = max;
			this.ErrorMessage = message;
		}

		public DecimalMaxAttribute(double max, string message)
		{
			this.Value = (decimal) max;
			this.ErrorMessage = message;
		}

		public decimal Value { get; set; }

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				return Convert.ToDecimal(value) <= Value;
			}
			catch (InvalidCastException)
			{
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

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;
            col.CheckConstraint = col.Name + "<=" + Convert.ToString(Value, CultureInfo.InvariantCulture);
		}

		#endregion
	}
}
