namespace NHibernate.Validator.Tests.Mappings
{
	public class CleanAddress
	{
		public static string blacklistedZipCode;

		public int floor;

		public CleanAddress()
		{
			InternalValid = true;
		}

		public long Id { get; set; }

		public string Country { get; set; }

		public string Line1 { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public string Line2 { get; set; }

		public bool InternalValid { get; set; }
	}
}