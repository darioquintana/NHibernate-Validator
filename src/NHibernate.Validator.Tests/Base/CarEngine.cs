using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class CarEngine
	{
		private long horsePower;

		[Pattern(Regex = "^[A-Z0-9-]+$", Message = "must contain alphabetical characters only")]
		[Pattern(Regex = "^....-....-....$", Message = "must match ....-....-....")]
		private string serialNumber;

		public string SerialNumber
		{
			get { return serialNumber; }
			set { serialNumber = value; }
		}

		public long HorsePower
		{
			get { return horsePower; }
			set { horsePower = value; }
		}
	}

	public class CarEngineDef: ValidationDef<CarEngine>
	{
		public CarEngineDef()
		{
			Define(x => x.SerialNumber).MatchWith("^[A-Z0-9-]+$").WithMessage("must contain alphabetical characters only").And.
				MatchWith("^....-....-....$").WithMessage("must match ....-....-....");
		}
	}
}