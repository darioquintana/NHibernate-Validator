using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg
{
	public interface IMappingSource
	{
		IClassMapping GetMapping();
	}
}