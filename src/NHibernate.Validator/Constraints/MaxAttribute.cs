using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
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

		public MaxAttribute() {}

		public MaxAttribute(long max)
		{
			Value = max;
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