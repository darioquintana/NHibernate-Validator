using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class LengthValidator : IInitializableValidator<LengthAttribute>, IPropertyConstraint
	{
		private int max;
		private int min;

		#region IInitializableValidator<LengthAttribute> Members

		public bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (!(value is string))
			{
				return false;
			}

			var @string = (string) value;
			int length = @string.Length;

			return length >= min && length <= max;
		}

		public void Initialize(LengthAttribute parameters)
		{
			min = parameters.Min;
			max = parameters.Max;
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column) ie.Current;

			if (max < int.MaxValue)
			{
				col.Length = max;
			}
		}

		#endregion
	}
}