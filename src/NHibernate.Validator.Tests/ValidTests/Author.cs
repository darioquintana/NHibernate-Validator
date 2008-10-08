using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ValidTests
{
	public class Author
	{
		private Blog blog;
		private string name;

		[NotNullNotEmpty]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		[Valid]
		public Blog Blog
		{
			get { return blog; }
			set { blog = value; }
		}
	}
}