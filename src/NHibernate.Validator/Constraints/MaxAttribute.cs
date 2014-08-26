using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Max restriction on a numeric annotated element
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class MaxAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public MaxAttribute() : this(0, "{validator.max}")
		{
		}

		public MaxAttribute(long max) : this(max, "{validator.max}")
		{
		}

		public MaxAttribute(long max, string message)
		{
			this.Value = max;
			this.ErrorMessage = message;
		}

		public long Value { get; set; }

		#region Implementation of IValidator

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			try
			{
				return Convert.ToDouble(value) <= Value;
			}
			catch (InvalidCastException)
			{
				if (value is char)
				{
					return Convert.ToInt32(value) <= Value;
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

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;
			col.CheckConstraint = col.Name + "<=" + Value;
		}

		#endregion
	}
}