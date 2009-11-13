using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated elemnt has to be in the appropriate range (excluding both limits).
	/// Apply on numeric values can be converted to double (<see cref="Convert.ToDouble(object)"/>).
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (WithinValidator))]
	public class WithinAttribute : EmbeddedRuleArgsAttribute, IRuleArgs
	{
		private string message = "{validator.within}";

		public WithinAttribute()
		{
			Max = double.MaxValue;
			Min = double.MinValue;
		}

		public WithinAttribute(double min, double max)
		{
			Min = min;
			Max = max;
		}

		public WithinAttribute(double min, double max, string message)
		{
			Min = min;
			Max = max;
			this.message = message;
		}

		public double Min { get; set; }

		public double Max { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}