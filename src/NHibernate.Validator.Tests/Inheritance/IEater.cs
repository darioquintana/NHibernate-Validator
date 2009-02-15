using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Inheritance
{
    public interface IEater
    {
        [Min(2)]
        int Frequency { get; set; }
    }

	public class IEaterDef : ValidationDef<IEater>
	{
		public IEaterDef()
		{
			Define(x => x.Frequency).GreaterThanOrEqualTo(2);
		}
	}
}