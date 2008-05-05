using System;
using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class LengthValidator : IInitializableValidator<LengthAttribute>, IPropertyConstraint
	{
		private int min;
		private int max;

		public bool IsValid(object value)
		{
			if(value == null) return true;
			if (!(value is string)) return false;

			string @string = (string) value;
			int length = @string.Length;

			return length >= min && length <= max;
		}

		public void Initialize(LengthAttribute parameters)
		{
			min = parameters.Min;
			max = parameters.Max;
		}

		public void Apply(Property property)
		{
			IEnumerator ie =  property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column col = (Column) ie.Current;
						
			if (max < int.MaxValue) 
				col.Length = max;
		}
	}
}