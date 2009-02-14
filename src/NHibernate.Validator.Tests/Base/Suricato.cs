using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Base
{
	[AssertAnimal]
	public class Suricato
	{
	}

	public class SuricatoDef:ValidationDef<Suricato>
	{
		public SuricatoDef()
		{
			ValidateInstance.Using(new AssertAnimalAttribute());
		}
	}
}