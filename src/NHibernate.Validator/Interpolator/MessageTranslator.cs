using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NHibernate.Validator.Interpolator
{
	public class MessageTranslator : IMessageTranslator
	{
		public static readonly Regex DefaultTokenizer = new Regex(@"(?<![#])[$]?{(\w|[._0-9])*}", RegexOptions.Compiled);
		public IEnumerable<string> RelevantTokens { get { return null; } }
		public string Replace(string[] relevantTokens, string[] values)
		{
			return null;
		}
	}
}