namespace NHibernate.Validator.Tests.CustomValidator
{
	public class Controller
	{
		private string _name;

		[NotNull]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _IP;

		public string IP
		{
			get { return _IP; }
			set { _IP = value; }
		}
	}
}
