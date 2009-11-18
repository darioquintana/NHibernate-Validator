using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a Date representation apply in the future
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class FutureAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		// TODO : Add tolerance
		private string message = "{validator.future}";

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