using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Inheritance
{
	public interface IName
	{
		[NotNull]
		string Name { get; set; }
	}

	public class INameDef : ValidationDef<IName>
	{
		public INameDef()
		{
			Define(x => x.Name).NotNullable();
		}
	}
}