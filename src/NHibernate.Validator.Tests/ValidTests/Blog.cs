using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ValidTests
{
	public class Blog
	{
		private Author author;
		private string title;

		[NotNullNotEmpty]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		[Valid]
		public Author Author
		{
			get { return author; }
			set { author = value; }
		}
	}
}