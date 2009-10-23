using System.Collections.Generic;

namespace NHibernate.Validator.Interpolator
{
	public interface IMessageTranslator
	{
		IEnumerable<string> RelevantTokens { get; }
		string Replace(string[] relevantTokens, string[] values);
	}
}