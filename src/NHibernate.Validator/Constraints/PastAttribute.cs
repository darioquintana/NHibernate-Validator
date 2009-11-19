using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a date representation apply in the past
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PastAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		public PastAttribute()
		{
			Message = "{validator.past}";
		}

		#region IRuleArgs Members

		public string Message { get; set; }

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			if (value is DateTime)
			{
				return DateTime.Now.CompareTo(value) > 0;
			}

			return false;
		}

		#endregion

	}
}