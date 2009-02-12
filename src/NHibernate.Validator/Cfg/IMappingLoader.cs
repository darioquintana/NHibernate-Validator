using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg
{
	public interface IMappingLoader
	{
		/// <summary>
		/// Load all mappings configured in the nhv-configuration configuration section.
		/// </summary>
		/// <param name="configurationMappings">The list of configured mappings.</param>
		void LoadMappings(IList<MappingConfiguration> configurationMappings);

		/// <summary>
		/// Adds all of the assembly's mappings.
		/// </summary>
		/// <param name="assemblyName">The assembly full name to load.</param>
		/// <seealso cref="Assembly.Load(AssemblyName)"/>
		void AddAssembly(string assemblyName);

		/// <summary>
		/// Adds all of the assembly's mappings.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		void AddAssembly(Assembly assembly);

		/// <summary>
		/// Get all mapped classes.
		/// </summary>
		/// <returns>All mapped classes for the current loader.</returns>
		IEnumerable<IClassMapping> GetMappings();
	}
}