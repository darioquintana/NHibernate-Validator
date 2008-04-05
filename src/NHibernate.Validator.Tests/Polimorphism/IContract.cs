namespace NHibernate.Validator.Tests.Polimorphism
{
	public interface IContract
	{
		[NotNull, Length(Max = 3)]
		string A { get; set; }
	}

	public class Impl : IContract
	{
		private string a;
		private string b;

		[NotNull, Length(Max = 3)]
		public string B
		{
			get { return b; }
			set { b = value; }
		}

		public string A
		{
			get { return a; }
			set { a = value; }
		}
	}
}