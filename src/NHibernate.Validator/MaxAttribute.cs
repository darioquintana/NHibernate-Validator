using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Max restriction on a numeric annotated element
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (MaxValidator))]
	public class MaxAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.max}";
		private long value;

		public MaxAttribute() {}

		public MaxAttribute(long max)
		{
			value = max;
		}

		public long Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}