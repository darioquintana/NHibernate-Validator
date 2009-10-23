using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NHibernate.Validator.Interpolator
{
	public class MessageTranslator : IMessageTranslator
	{
		public static readonly Regex DefaultTokenizer = new Regex(@"(?<![#])[$]?{(\w|[._0-9])*}", RegexOptions.Compiled);
		public static readonly Regex EscapeReplacement = new Regex("#{", RegexOptions.Compiled);
		private IEnumerable<string> relevantTokens;

		public MessageTranslator(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			Message = message;
		}

		public string Message { get; private set; }

		#region IMessageTranslator Members

		public IEnumerable<string> RelevantTokens
		{
			get
			{
				if (relevantTokens == null)
				{
					relevantTokens = DefaultTokenizer.Matches(Message).Cast<Match>().Select(m => m.Value).Distinct();
				}
				return relevantTokens;
			}
		}

		public string Replace(IEnumerable<KeyValuePair<string, string>> replacements)
		{
			string result = Message;
			if (replacements != null)
			{
				foreach (var valuePair in replacements)
				{
					result = DefaultTokenizer.Replace(result,
					                                  match => match.Value.Equals(valuePair.Key) ? valuePair.Value ?? string.Empty : match.Value);
				}
			}

			return EscapeReplacement.Replace(result, "{");
		}

		#endregion
	}
}