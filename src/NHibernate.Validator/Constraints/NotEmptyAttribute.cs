using System;
using System.Collections;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check that a String is not empty (length > 0)
	/// or that a IEnumerable is not empty (Count > 0)
	/// </summary>
	/// <remarks>
	/// To validate for NotNull and Not Empty use NotNullNotEmptyAttribute.
	/// </remarks>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class NotEmptyAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private string message = "{validator.notEmpty}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			var check = value as string;
			if (check != null)
			{
				return !string.Empty.Equals(check.Trim());
			}

			var ev = value as IEnumerable;
			return ev != null && ev.Any();
		}

		#endregion
	}
}