using System;
using System.Collections;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Ensure a IEnumerable (including string) to be not null and not empty.
	/// <code>
	/// <example>
	/// Valid values: "abc"; new int[] {1}; new List&lt;int>(new int[] { 1 });
	/// Invalid values: null; ""; 123; new int[0]; new List&lt;int>();
	/// </example>
	/// </code>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NotNullNotEmptyAttribute : NotNullAttribute
	{
		public NotNullNotEmptyAttribute()
		{
			Message = "{validator.notNullNotEmpty}";
		}

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value is Guid)
			{
				return !Guid.Empty.Equals(value);
			}
 
			var ev = value as IEnumerable;
			if (ev != null)
			{
				return ev.Any();
			}

			return false;
		}
	}
}