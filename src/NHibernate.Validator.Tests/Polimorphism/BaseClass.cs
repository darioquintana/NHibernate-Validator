using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Polimorphism
{
	public interface IInheritance
	{
		
	}
	public class BaseClass : IInheritance
	{
		[Length(3)]
		public string A;
	}

	public class DerivatedClass : BaseClass
	{
		[Length(3)]
		public string B;
	}

	public class Composition
	{
		[Valid]
		public IInheritance Any;
	}
}
