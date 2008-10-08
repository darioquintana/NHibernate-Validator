using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Min restriction on a numeric annotated element (or the string representation of a numeric)
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (MinValidator))]
	public class MinAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.min}";

		public MinAttribute() {}

		public MinAttribute(long min)
		{
			Value = min;
		}

		public long Value { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}