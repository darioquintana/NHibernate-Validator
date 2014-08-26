using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Apply some length restrictions to the annotated element. It has to be a string
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class LengthAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public LengthAttribute(int min, int max, string message)
		{
			this.Min = min;
			this.Max = max;
			this.ErrorMessage = message;
		}

		public LengthAttribute(int min, int max) : this(min, max, "{validator.length}")
		{
		}

		public LengthAttribute(int max) : this(0, max, "{validator.length}")
		{
		}

		public LengthAttribute() : this(0, int.MaxValue, "{validator.length}")
		{
		}

		public int Min { get; set; }

		public int Max { get; set; }

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}
			if (!(value is string))
			{
				return false;
			}

			var @string = (string)value;
			int length = @string.Length;

			return length >= Min && length <= Max;
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;

			if (Max < int.MaxValue)
			{
				col.Length = Max;
			}
		}

		#endregion
	}
}