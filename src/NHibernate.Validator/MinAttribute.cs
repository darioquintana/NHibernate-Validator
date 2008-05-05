using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Min restriction on a numeric annotated elemnt (or the string representation of a numeric)
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (MinValidator))]
	public class MinAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.min}";
		private long value;

		public MinAttribute() {}

		public MinAttribute(long min)
		{
			value = min;
		}

		public long Value
		{
			get { return value; }
			set { this.value = value; }
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