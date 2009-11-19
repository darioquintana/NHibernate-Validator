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
	public class RangeAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
	{
		private long max = long.MaxValue;
		private string message = "{validator.range}";
		private long min = long.MinValue;

		public RangeAttribute(long min, long max)
		{
			this.min = min;
			this.max = max;
		}

		public RangeAttribute(long min, long max, string message)
		{
			this.min = min;
			this.max = max;
			this.message = message;
		}

		public RangeAttribute() {}

		public long Min
		{
			get { return min; }
			set { min = value; }
		}

		public long Max
		{
			get { return max; }
			set { max = value; }
		}

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region Implementation of IValidator

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
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


		#endregion

		#region Implementation of IPropertyConstraint

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;

			String check = "";
			if (min != long.MinValue)
			{
				check += col.Name + ">=" + min;
			}
			if (max != long.MaxValue && min != long.MinValue)
			{
				check += " and ";
			}
			if (max != long.MaxValue)
			{
				check += col.Name + "<=" + max;
			}
			col.CheckConstraint = check;
		}

		#endregion
	}
}