using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class User
	{
		[NotNull] private String firstName;

		[NotNull] //(groups = Default.class)
			private String lastName;

		[Valid] private List<Address> addresses = new List<Address>();

		[Valid] private List<User> knowsUser = new List<User>();

		public User()
		{
		}

		public User(String firstName, String lastName)
		{
			this.firstName = firstName;
			this.lastName = lastName;
		}

		public List<Address> getAddresses()
		{
			return addresses;
		}

		public void addAddress(Address address)
		{
			addresses.Add(address);
		}

		public void knows(User user)
		{
			knowsUser.Add(user);
		}

		public List<User> getKnowsUser()
		{
			return knowsUser;
		}

		public String getFirstName()
		{
			return firstName;
		}

		public void setFirstName(String firstName)
		{
			this.firstName = firstName;
		}

		public String getLastName()
		{
			return lastName;
		}

		public void setLastName(String lastName)
		{
			this.lastName = lastName;
		}
	}
}