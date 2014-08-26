using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a date representation apply in the past
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PastAttribute : EmbeddedRuleArgsAttribute
	{
		public PastAttribute()
		{
			this.ErrorMessage = "{validator.past}";
		}

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext constraintContext)
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