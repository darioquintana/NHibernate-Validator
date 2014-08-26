using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated elemnt has to be in the appropriate range. Apply on numeric values or string
	/// representation of the numeric value.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class RangeAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public RangeAttribute(long min, long max) : this(min, max, "{validator.range}")
		{
		}

		public RangeAttribute(long min, long max, string message)
		{
			this.Min = min;
			this.Max = max;
			this.ErrorMessage = message;
		}

		public RangeAttribute() : this(long.MinValue, long.MaxValue, "{validator.range}")
		{
		}

		public long Min
		{
			get;
			set;
		}

		public long Max
		{
			get;
			set;
		}

		#region Implementation of IValidator

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				double cvalue = Convert.ToDouble(value);
				return cvalue >= this.Min && cvalue <= this.Max;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					int i = Convert.ToInt32(value);
					return i >= this.Min && i <= this.Max;
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
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;

			String check = "";
			if (this.Min != long.MinValue)
			{
				check += col.Name + ">=" + this.Min;
			}
			if (this.Max != long.MaxValue && this.Min != long.MinValue)
			{
				check += " and ";
			}
			if (this.Max != long.MaxValue)
			{
				check += col.Name + "<=" + this.Max;
			}
			col.CheckConstraint = check;
		}

		#endregion
	}
}