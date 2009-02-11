using System;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	public class KnownRules
	{
		public string AP { get; set; }
		public string StrProp { get; set; }
		public DateTime DtProp { get; set; }
		public decimal DecProp { get; set; }
		public bool BProp { get; set; }
		public int[] ArrProp { get; set; }
		public string Pattern { get; set; }
	}
}