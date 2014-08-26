using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated element has to be true
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class AssertTrueAttribute : EmbeddedRuleArgsAttribute
	{
		public AssertTrueAttribute()
		{
			this.ErrorMessage = "{validator.assertTrue}";
		}
		
		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return false;
			}

			if (value is bool)
			{
				return (bool)value;
			}

			return false;
		}

		#endregion
	}
}