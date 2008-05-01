using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Size range for Arrays, Collections
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (SizeValidator))]
	public class SizeAttribute : Attribute, IRuleArgs
	{
		private int max = int.MaxValue;
		private string message = "{validator.size}";
		private int min = 0;

		public SizeAttribute() {}

		public SizeAttribute(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public int Min
		{
			get { return min; }
			set { min = value; }
		}

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