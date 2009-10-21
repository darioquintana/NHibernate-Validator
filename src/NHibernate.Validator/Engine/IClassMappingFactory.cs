using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Engine
{
	public interface IClassMappingFactory
	{
		IClassMapping GetClassMapping(System.Type clazz, ValidatorMode mode);
	}
}