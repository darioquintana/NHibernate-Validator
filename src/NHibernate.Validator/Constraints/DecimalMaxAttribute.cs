using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Max restriction on a numeric annotated element
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(DecimalMaxValidator))]
	public class DecimalMaxAttribute : EmbeddedRuleArgsAttribute, IRuleArgs
	{
		private string message = "{validator.max}";

		public DecimalMaxAttribute() { }

		public DecimalMaxAttribute(decimal max)
		{
			Value = max;
		}

		public decimal Value { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}
