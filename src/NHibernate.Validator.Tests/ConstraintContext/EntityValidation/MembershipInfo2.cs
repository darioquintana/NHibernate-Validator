namespace NHibernate.Validator.Tests.ConstraintContext.EntityValidation
{
	public class MembershipInfo2 : IMembershipInfo
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	[Password2]
	public interface IMembershipInfo
	{
		string Username { get; set; }
		string Password { get; set; }
	}
}