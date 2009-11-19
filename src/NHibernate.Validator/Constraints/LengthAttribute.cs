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
	public class LengthAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
	{
		private string message = "{validator.length}";

		public LengthAttribute(int min, int max) : this(max)
		{
			Min = min;
		}

		public LengthAttribute(int max)
		{
			Max = max;
		}

		public LengthAttribute()
		{
			Max = int.MaxValue;
		}
		public int Min { get; set; }

		public int Max { get; set; }

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