using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.DelegatedEntityValidator
{
	public class Range
	{
		public int Start { get; set; }
		public int End { get; set; }
	}

	public class RangeDef : ValidationDef<Range>
	{
		public RangeDef()
		{
			ValidateInstance.By((instance, context) => instance.Start <= instance.End).WithMessage("Start should be less than End.");
		}
	}
}