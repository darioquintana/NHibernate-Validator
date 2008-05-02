namespace NHibernate.Validator.Tests.ValidatorsTest
{
	public class FooNotEmpty
	{
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		public FooNotEmpty(string message)
		{
			this.message = message;
		}

		[NotNullNotEmpty]
		private string message;
	}
}