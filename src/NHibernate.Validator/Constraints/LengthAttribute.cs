using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Apply some length restrictions to the annotated element. It has to be a string
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (LengthValidator))]
	public class LengthAttribute : EmbeddedRuleArgsAttribute, IRuleArgs
	{
		private int max = int.MaxValue;
		private string message = "{validator.length}";

		public LengthAttribute(int min, int max) : this(max)
		{
			Min = min;
		}

		public LengthAttribute(int max)
		{
			this.max = max;
		}

		public LengthAttribute() {}

		public int Min { get; set; }

		public int Max
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