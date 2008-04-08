using System;

namespace NHibernate.Validator.Demo.Winforms.Model
{
	public class Customer
	{
		private int id;
		private string firstName;
		private string lastName;
		private DateTime born;
		private string zip;
		private string email;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		[NotEmpty,NotNull]
		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		[NotEmpty, NotNull]
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		[Past]
		public DateTime Born
		{
			get { return born; }
			set { born = value; }
		}
		
		[Zip(Message = "wrong zip code")]
		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}

		[Email]
		public string Email
		{
			get { return email; }
			set { email = value; }
		}
	}
}