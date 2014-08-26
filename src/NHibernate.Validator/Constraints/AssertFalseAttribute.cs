using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated element has to be false
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class AssertFalseAttribute : EmbeddedRuleArgsAttribute
	{
		public AssertFalseAttribute()
		{
			this.ErrorMessage = "{validator.assertFalse}";
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
				return !(bool)value;
			}

			return false;
		}

		#endregion
	}
}