using System.Collections.Generic;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg
{
	public interface IMappingsProvider
	{
		/// <summary>
		/// Get all mapped classes.
		/// </summary>
		/// <returns>All mapped classes for the current loader.</returns>
		IEnumerable<IClassMapping> GetMappings();
	}
}