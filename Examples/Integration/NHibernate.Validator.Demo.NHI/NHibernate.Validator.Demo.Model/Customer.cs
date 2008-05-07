using System;

namespace NHibernate.Validator.Demo.Model
{
	public class Customer
	{
		private DateTime born;
		private string email;
		private string firstName;
		private int id;
		private string lastName;
		private string phone;
		private string zip;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public DateTime Born
		{
			get { return born; }
			set { born = value; }
		}

		public string Phone
		{
			get { return phone; }
			set { phone = value; }
		}

		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}
	}
}