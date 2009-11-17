using System;
using System.Collections.Generic;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class BaseMappingsProvider : IMappingsProvider
	{
		private readonly List<IClassMapping> classMappings = new List<IClassMapping>();

		#region IMappingsProvider Members

		IEnumerable<IClassMapping> IMappingsProvider.GetMappings()
		{
			return classMappings;
		}

		#endregion

		[CLSCompliant(false)]
		public void Add<T>(IValidationDefinition<T> definition) where T : class
		{
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			var ms = definition as IMappingSource;
			if (ms == null)
			{
				throw new ArgumentException("The argument is not an implementation of " + typeof (IMappingSource).FullName,
				                            "definition");
			}
			classMappings.Add(ms.GetMapping());
		}
	}
}