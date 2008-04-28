namespace NHibernate.Validator.Tests.Base
{
	public class AClass
	{
		[Length(3)]
		public string A;
	}

	public class Composition
	{
		[Valid]
		public Polimorphism.IInheritance Any;
	}
}