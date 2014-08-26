using System;
using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
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
	public class SizeAttribute : EmbeddedRuleArgsAttribute
	{
		private int max = int.MaxValue;

		/// <summary>
		/// Min = 0
		/// Max = int.MaxValue
		/// </summary>
		public SizeAttribute()
		{
			this.ErrorMessage = "{validator.size}";
		}

		/// <summary>
		/// Min and Max are specified in the parameters.
		/// </summary>
		/// <param name="min">Min value to ensure</param>
		/// <param name="max">Max value to ensure</param>
		public SizeAttribute(int min, int max)
		{
			Min = min;
			this.max = max;
		}

		public int Min { get; set; }

		public int Max
		{
			get { return max; }
			set { max = value; }
		}

		#region Implementation of IValidator

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			int count = 0;
			var collection = value as ICollection;
			if (collection != null)
			{
				count = collection.Count;
			}
			else
			{
				var enumerable = value as IEnumerable;
				if (ReferenceEquals(null, enumerable))
				{
					return false;
				}
				var enumerator = enumerable.GetEnumerator();
				while (enumerator.MoveNext() && count <= Max)
				{
					count++;
				}
			}
			return count >= Min && count <= Max;
		}

		#endregion
	}
}