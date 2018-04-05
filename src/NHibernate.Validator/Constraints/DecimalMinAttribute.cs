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
	public class DecimalMinAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
	{
		private string message = "{validator.min}";

		public DecimalMinAttribute () { }

		public DecimalMinAttribute(int min) : this(new decimal(min))
		{
		}

		[CLSCompliant(false)]
		public DecimalMinAttribute(uint min) : this(new decimal(min))
		{
		}

		public DecimalMinAttribute(long min) : this(new decimal(min))
		{
		}

		[CLSCompliant(false)]
		public DecimalMinAttribute(ulong min) : this(new decimal(min))
		{
		}

		public DecimalMinAttribute(float min) : this(new decimal(min))
		{
		}

		public DecimalMinAttribute(double min) : this(new decimal(min))
		{
		}

		public DecimalMinAttribute(int lo, int mid, int hi, bool isNegative, byte scale)
			: this(new decimal(lo, mid, hi, isNegative, scale))
		{
		}

		public DecimalMinAttribute(decimal min)
		{
			Value = min;
		}

		public decimal Value { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
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
