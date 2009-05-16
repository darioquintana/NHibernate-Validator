using System;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Demo.Winforms.Model
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

		[NotEmpty, NotNull]
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

		//This is a custom validator
		[Phone(Message = "Wrong phone number. Examples of valid matches: 800-555-5555 | 333-444-5555 | 212-666-1234")]
		public string Phone
		{
			get { return phone; }
			set { phone = value; }
		}

		[Email]
		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		[Pattern(Regex = "^[A-Z0-9-]+$", Message = "Examples of valid matches: 234G-34DA | 3432-DF23")]
		[Pattern(Regex = "^....-....$", Message = "Must match ....-....")]
		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}
	}
}