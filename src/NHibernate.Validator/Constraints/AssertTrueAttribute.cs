using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated element has to be true
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class AssertTrueAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private string message = "{validator.assertTrue}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
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