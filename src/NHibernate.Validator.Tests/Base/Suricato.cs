using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Base
{
	[AssertAnimal(Message = "is not an animal")]
	public class Suricato
	{
	}

	public class SuricatoDef:ValidationDef<Suricato>
	{
		public SuricatoDef()
		{
			var attribute = new AssertAnimalAttribute();
			attribute.Message = "is not an animal";
			ValidateInstance.Using(attribute);
		}
	}
}