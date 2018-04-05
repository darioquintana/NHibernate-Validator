using System;
using System.Collections.Generic;

using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Engine
{
	internal class StateFullClassMappingFactory : JITClassMappingFactory
	{
		private const string duplicationWarnMessageTemplate =
			@"Duplicated external definition for class {0}.
Possible causes:
- There are more than one definitions for the class.
- You have more than one time the same mapping.
Note: 'external' mean XML or any other mapping source than Attribute.";

#if NETFX
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(StateFullClassMappingFactory));
#else
		private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(StateFullClassMappingFactory));
#endif

		private readonly Dictionary<System.Type, IClassMapping> definitions = new Dictionary<System.Type, IClassMapping>();

		public void AddClassExternalDefinition(IClassMapping definition)
		{
			System.Type type = definition.EntityType;
#if NETFX
			log.Debug("Adding external definition for " + type.FullName);
#else
			Log.Debug("Adding external definition for {0}", type.FullName);
#endif
			try
			{
				definitions.Add(type, definition);
			}
			catch (ArgumentException)
			{
#if NETFX
				log.Warn(string.Format(duplicationWarnMessageTemplate, type.AssemblyQualifiedName));
#else
				Log.Warn(duplicationWarnMessageTemplate, type.AssemblyQualifiedName);
#endif
			}
		}

		protected override IClassMapping GetExternalDefinitionFor(System.Type type)
		{
			IClassMapping result;
			definitions.TryGetValue(type, out result);
			return result;
		}

		public IEnumerable<System.Type> GetLoadedDefinitions()
		{
			return definitions.Keys;
		}
	}
}
