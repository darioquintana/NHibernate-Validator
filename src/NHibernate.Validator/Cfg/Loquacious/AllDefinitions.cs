using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public static class AllDefinitions
	{
		public static IEnumerable<System.Type> FromAssembly(string assemblyName)
		{
			return Assembly.Load(assemblyName).ValidationDefinitions();
		}

		public static IEnumerable<System.Type> ValidationDefinitions(this Assembly assembly)
		{
			var assemblyTypes = assembly.GetTypes();
			return assemblyTypes.Where(x => typeof(IMappingSource).IsAssignableFrom(x));
		}

		public static IEnumerable<System.Type> ValidationDefinitions(this IEnumerable<System.Type> types)
		{
			return types.Where(x => typeof(IMappingSource).IsAssignableFrom(x));
		}
	}
}