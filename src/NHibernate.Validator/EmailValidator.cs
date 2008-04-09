using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class EmailValidator : IInitializableValidator<EmailAttribute>
	{
		private const string ATOM = "[^\\x00-\\x1F^\\(^\\)^\\<^\\>^\\@^\\,^\\(;^\\:^\\\\^\\\"^\\.^\\[^\\]^\\s]";
		private const string DOMAIN = "(" + ATOM + "+(\\." + ATOM + "+)*";
		private const string IP_DOMAIN = "\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]";
		private Regex regex;

		public bool IsValid(object value) 
		{
			if (value == null) return true;
			if (!(value is string)) return false;
			string @string = (String) value;
			if ( @string.Length == 0 ) return true;
			return regex.IsMatch(@string);
		}

		public void Initialize(EmailAttribute parameters)
		{
			regex = new Regex(string.Concat("^",ATOM, "+(\\." , ATOM , "+)*@" , DOMAIN , "|" , IP_DOMAIN , ")$"), RegexOptions.Compiled);
		}
	}
}