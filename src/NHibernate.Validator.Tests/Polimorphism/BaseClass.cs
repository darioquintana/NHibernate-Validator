namespace NHibernate.Validator.Tests.Polimorphism
{
	public class BaseClass
	{
		[Length(3)]
		public string A;
	}

	public class DerivatedClass : BaseClass
	{
		[Length(3)]
		public string B;
	}
}
