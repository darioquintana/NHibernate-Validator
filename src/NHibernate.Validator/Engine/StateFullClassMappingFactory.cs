using System;
using System.Collections.Generic;
using log4net;
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

		private static readonly ILog log = LogManager.GetLogger(typeof(StateFullClassMappingFactory));

		private readonly Dictionary<System.Type, IClassMapping> definitions = new Dictionary<System.Type, IClassMapping>();

		public void AddClassExternalDefinition(IClassMapping definition)
		{
			System.Type type = definition.EntityType;
			log.Debug("Adding external definition for " + type.FullName);
			try
			{
				definitions.Add(type, definition);
			}
			catch (ArgumentException)
			{
				log.Warn(string.Format(duplicationWarnMessageTemplate, type.AssemblyQualifiedName));
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
