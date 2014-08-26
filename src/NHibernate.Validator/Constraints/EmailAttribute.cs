using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The string has to be a well-formed email address
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class EmailAttribute : EmbeddedRuleArgsAttribute
	{
		private const string ATOM = "[^\\x00-\\x1F^\\(^\\)^\\<^\\>^\\@^\\,^\\;^\\:^\\\\^\\\"^\\.^\\[^\\]^\\s]";
		private const string DOMAIN = "(" + ATOM + "+(\\." + ATOM + "+)*";
		private const string IP_DOMAIN = "\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]";

		private readonly Regex regex = new Regex(
			string.Concat("^", ATOM, "+(\\.", ATOM, "+)*@", DOMAIN, "|", IP_DOMAIN, ")$"), RegexOptions.Compiled);

		public EmailAttribute()
		{
			this.ErrorMessage = "{validator.email}";
		}

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}
			var mail = value as string;
			if (mail == null)
			{
				return false;
			}
			if (mail.Length == 0)
			{
				return true;
			}
			return regex.IsMatch(mail);
		}

		#endregion
	}
}