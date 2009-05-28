using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ConstraintContext.MemberValidation
{
	public class MembershipInfo
	{
		[NotNull]
		public string Username;

		[Password(Message = "Invalid password")]
		public string Password;
	}
}