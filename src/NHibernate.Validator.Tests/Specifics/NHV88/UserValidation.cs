using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Specifics.NHV88
{
	public class UserValidation : ValidationDef<User>
	{
		public UserValidation()
		{
			Define(x => x.Name)
				.NotNullableAndNotEmpty();
			Define(x => x.Group)
				.NotNullable()
				.And
				.IsValid();
		}
	}
}