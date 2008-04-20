namespace NHibernate.Validator.Tests.ValidTests
{
	public class Address
	{
		private string city;

		[NotNull]
		public string City
		{
			get { return city; }
			set { city = value; }
		}
	}
}