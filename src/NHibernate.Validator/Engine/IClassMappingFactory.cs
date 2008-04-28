using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Engine
{
	internal interface IClassMappingFactory
	{
		IClassMapping GetClassMapping(System.Type clazz, ValidatorMode mode);
	}
}