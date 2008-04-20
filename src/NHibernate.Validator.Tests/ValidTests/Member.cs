using NHibernate.Validator.Tests.ValidTests;

namespace NHibernate.Validator.Tests.ValidTests
{
	public class Member
	{
		private Address _address;

		[Valid]
		public Address Address
		{
			get { return _address; }
			set { _address = value; }
		}
	}
}