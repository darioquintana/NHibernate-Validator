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

			var checkString = value as string;
			if (checkString != null)
			{
				return !string.Empty.Equals(checkString.Trim());
			}
			if (value is Guid)
			{
				return !Guid.Empty.Equals(value);
			}

			var ev = value as IEnumerable;
			return ev != null && ev.Any();
		}

		#endregion
	}
}