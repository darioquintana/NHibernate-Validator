namespace NHibernate.Validator.Tests.Integration
{
	[WrongConstraint]
	public class WrongClass
	{
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
	}

	public class WrongClass1
	{
		private int id;
		[WrongConstraint]
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
	}
}
