namespace NHibernate.Validator.Tests.ConstraintContext
{
	internal class Messages
	{
		public const string PasswordLength = "The password has should be larger than 5";
		public const string PasswordContent = "The password can't have the '123' phrase";
		public const string PasswordContentUsername = "The password can't contains the Username";
	}
}