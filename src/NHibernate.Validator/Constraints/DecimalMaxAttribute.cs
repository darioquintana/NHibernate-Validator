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
	public class DecimalMaxAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
	{
		private string message = "{validator.max}";

		public DecimalMaxAttribute() { }

		public DecimalMaxAttribute(int min) : this(new decimal(min))
		{
		}

		[CLSCompliant(false)]
		public DecimalMaxAttribute(uint min) : this(new decimal(min))
		{
		}

		public DecimalMaxAttribute(long min) : this(new decimal(min))
		{
		}

		[CLSCompliant(false)]
		public DecimalMaxAttribute(ulong min) : this(new decimal(min))
		{
		}

		public DecimalMaxAttribute(float min) : this(new decimal(min))
		{
		}

		public DecimalMaxAttribute(double min) : this(new decimal(min))
		{
		}

		public DecimalMaxAttribute(int lo, int mid, int hi, bool isNegative, byte scale)
			: this(new decimal(lo, mid, hi, isNegative, scale))
		{
		}

		public DecimalMaxAttribute(decimal max)
		{
			Value = max;
		}

		public decimal Value { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
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
