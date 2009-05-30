using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class User
	{
		[NotNullNotEmpty] public string FirstName { get; set; }

		[NotNullNotEmpty] //(groups = Default.class)
		public string LastName { get; set; }

		[Valid] private List<Address> addresses = new List<Address>();

		[Valid] private List<User> knowsUser = new List<User>();

		public User()
		{
		}

		public User(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}

		public List<Address> Addresses()
		{
			return addresses;
		}

		public void AddAddress(Address address)
		{
			addresses.Add(address);
		}

		public void Knows(User user)
		{
			knowsUser.Add(user);
		}

		public List<User> KnowsUsers
		{
			get{ return knowsUser; }
		}
	}
}