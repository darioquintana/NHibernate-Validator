using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a Date representation apply in the future
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class FutureAttribute : EmbeddedRuleArgsAttribute
	{
		// TODO : Add tolerance
		public FutureAttribute()
		{
			this.ErrorMessage = "{validator.future}";
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
				return DateTime.Now.CompareTo(value) < 0;
			}

			return false;
		}

		#endregion
	}
}