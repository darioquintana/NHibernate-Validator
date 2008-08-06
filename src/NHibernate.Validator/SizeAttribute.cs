using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Size range for Arrays, Collections (do not use with strings).
	/// <code>
	/// <example>
	/// -[SizeAttribute] range from 0 to int.MaxValue.
	/// Valid values: new int[0];
	/// 
	/// [SizeAttribute(1,3)] range from 1 to 3.
	/// Valid values:  new int[1]; new int[3]
	/// Invalid values: new int[0];  new int[4]; "123"; 123;
	/// </example>
	/// </code>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (SizeValidator))]
	public class SizeAttribute : Attribute, IRuleArgs
	{
		private int max = int.MaxValue;
		private string message = "{validator.size}";
		private int min = 0;

		/// <summary>
		/// Min = 0
		/// Max = int.MaxValue
		/// </summary>
		public SizeAttribute() {}

		/// <summary>
		/// Min and Max are specified in the parameters.
		/// </summary>
		/// <param name="min">Min value to ensure</param>
		/// <param name="max">Max value to ensure</param>
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