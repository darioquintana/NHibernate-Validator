using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// The annotated elemnt has to be in the appropriate range. Apply on numeric values or string
	/// representation of the numeric value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(RangeValidator))]
	public class RangeAttribute : Attribute, IRuleArgs
	{
		private long min = long.MinValue;
		private long max = long.MaxValue;
		private string message = "{validator.range}";

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

		public RangeAttribute()
		{
		}

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

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}