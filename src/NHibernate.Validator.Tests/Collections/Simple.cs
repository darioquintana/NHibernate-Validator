using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	public class Simple
	{		
		[NotNull] public string name;

		public Simple()
		{
		}

		public Simple(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name ?? "null";
		}
	}

	public class SimpleDef : ValidationDef<Simple>
	{
		public SimpleDef()
		{
			Define(x => x.name).NotNullable();
		}
	}
}
