using System;
using System.Collections.Generic;
using log4net;
using NHibernate.Util;
using NHibernate.Validator.Cfg.MappingSchema;

namespace NHibernate.Validator.Engine
{
	internal class StateFullClassMappingFactory : JITClassMappingFactory
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorEngine));

		private readonly Dictionary<System.Type, NhvmClass> definitions= new Dictionary<System.Type, NhvmClass>();

		public void AddClassDefinition(NhvmClass definition)
		{
			if (definition == null) return;
			System.Type type =
				ReflectHelper.ClassForFullName(
					TypeNameParser.Parse(definition.name, definition.rootMapping.@namespace, definition.rootMapping.assembly).ToString());
			try
			{
				definitions.Add(type, definition);
			}
			catch (ArgumentException)
			{
				log.Warn("Duplicated XML definition for class " + type.AssemblyQualifiedName);
			}
		}

		protected override NhvmClass GetXmlDefinitionFor(System.Type type)
		{
			NhvmClass result;
			definitions.TryGetValue(type, out result);
			return result;
		}

		public IEnumerable<System.Type> GetLoadedDefinitions()
		{
			return definitions.Keys;
		}
	}
}
