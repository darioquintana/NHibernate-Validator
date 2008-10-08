using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class EmailValidator : IValidator
	{
		private const string ATOM = "[^\\x00-\\x1F^\\(^\\)^\\<^\\>^\\@^\\,^\\;^\\:^\\\\^\\\"^\\.^\\[^\\]^\\s]";
		private const string DOMAIN = "(" + ATOM + "+(\\." + ATOM + "+)*";
		private const string IP_DOMAIN = "\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]";

		private readonly Regex regex = new Regex(
			string.Concat("^", ATOM, "+(\\.", ATOM, "+)*@", DOMAIN, "|", IP_DOMAIN, ")$"), RegexOptions.Compiled);

		#region IValidator Members

		public bool IsValid(object value)
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