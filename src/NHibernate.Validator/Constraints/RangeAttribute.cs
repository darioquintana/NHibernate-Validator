using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated elemnt has to be in the appropriate range. Apply on numeric values or string
	/// representation of the numeric value.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (RangeValidator))]
	public class RangeAttribute : Attribute, IRuleArgs
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
	}
}