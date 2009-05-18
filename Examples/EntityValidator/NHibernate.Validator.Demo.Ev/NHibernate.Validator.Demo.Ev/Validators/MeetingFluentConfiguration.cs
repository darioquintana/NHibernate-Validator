using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Demo.Ev.Model;

namespace NHibernate.Validator.Demo.Ev.Validators
{
	public class MeetingDef : ValidationDef<Meeting>
	{
		public MeetingDef()
		{
			Define(x => x.Name).NotNullableAndNotEmpty();
			Define(x => x.Description).NotNullableAndNotEmpty();
			
			ValidateInstance.Using(new ValidRangeAttribute());
		}
	}
}