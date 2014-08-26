using System;
using System.Globalization;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check if string is a number.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class IsNumericAttribute : EmbeddedRuleArgsAttribute
	{
		public IsNumericAttribute()
		{
			this.ErrorMessage = "{validator.numeric}";
		}

		#region Implementation of IValidator

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			return value == null || (value is string && IsNumeric(value));
		}

		#endregion

		private static bool IsNumeric(object Expression)
		{
			double retNum;

			return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
		}
	}
}