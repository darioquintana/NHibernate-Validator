using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Specifics.NHV88
{
	public class GroupValidation : ValidationDef<Group>
	{
		public GroupValidation()
		{
			Define(x => x.Name)
				.NotNullableAndNotEmpty();
		}
	}
}