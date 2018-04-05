using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class FluentMappingLoader : IMappingLoader
	{
#if NETFX
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(FluentMappingLoader));
#else
		private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(FluentMappingLoader));
#endif
		private readonly List<IClassMapping> classMappings = new List<IClassMapping>();

		public void LoadMappings(IList<MappingConfiguration> configurationMappings)
		{
			if (configurationMappings == null)
			{
				throw new ArgumentNullException("configurationMappings");
			}

			foreach (var mc in configurationMappings)
			{
				if (!string.IsNullOrEmpty(mc.Assembly) && string.IsNullOrEmpty(mc.Resource))
				{
#if NETFX
					log.Debug("Assembly " + mc.Assembly);
#else
					Log.Info("Assembly {0}", mc.Assembly);
#endif
					AddAssembly(mc.Assembly);
				}
				else if (!string.IsNullOrEmpty(mc.Assembly) && !string.IsNullOrEmpty(mc.Resource))
				{
#if NETFX
					log.Debug("Class " + mc.Resource + " in " + mc.Assembly);
#else
					Log.Debug("Class {0} in {1}", mc.Resource, mc.Assembly);
#endif
					AddClassDefinition(Assembly.Load(mc.Assembly), mc.Resource);
				}
				else
				{
#if NETFX
					log.Warn(string.Format("Mapping configuration ignored: Assembly>{0}< Resource>{1}< File>{2}<", mc.Assembly,
					                       mc.Resource, mc.File));
#else
					Log.Warn("Mapping configuration ignored: Assembly >{0}< Resource >{1}< File >{2}<", mc.Assembly,
					         mc.Resource, mc.File);
#endif
				}
			}
		}

		public void AddAssembly(string assemblyName)
		{
#if NETFX
			log.Info("Searching for mapped documents in assembly: " + assemblyName);
#else
			Log.Info("Searching for mapped documents in assembly: {0}", assemblyName);
#endif

			Assembly assembly;
			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not add assembly " + assemblyName, e);
			}

			AddAssembly(assembly);
		}

		public void AddAssembly(Assembly assembly)
		{
			var assemblyTypes = assembly.GetTypes();
			var types = assemblyTypes.Where(x => typeof (IMappingSource).IsAssignableFrom(x));
			foreach (System.Type type in types)
			{
				AddClassDefinition(type);
			}
		}

		public void AddNameSpace(Assembly assembly, string nameSpace)
		{
			var assemblyTypes = assembly.GetTypes();
			AddClassDefinitions(assemblyTypes.Where(x => x.Namespace.StartsWith(nameSpace)));
		}

		public void AddClassDefinitions(IEnumerable<System.Type> definitions)
		{
			var types = definitions.Where(x => typeof(IMappingSource).IsAssignableFrom(x));
			foreach (System.Type type in types)
			{
				AddClassDefinition(type);
			}
		}

		public void AddClassDefinition(Assembly assembly, string classFullName)
		{
			try
			{
				var t = assembly.GetType(classFullName, true);
				AddClassDefinition(t);
			}
			catch (Exception ex)
			{
				throw new ValidatorConfigurationException(ex);
			}
		}

		[CLSCompliant(false)]
		public void AddClassDefinition<TDef, T>() where TDef : IValidationDefinition<T>, new() where T : class
		{
			AddClassDefinition(new TDef());
		}

		private void AddClassDefinition(System.Type type)
		{
			try
			{
				var ms = (IMappingSource) Activator.CreateInstance(type);
				classMappings.Add(ms.GetMapping());
			}
			catch (MissingMethodException ex)
			{
				throw new ValidatorConfigurationException("Public constructor was not found for fluent mapping : " + type, ex);
			}
			catch (InvalidCastException ex)
			{
				throw new ValidatorConfigurationException(
					"Type does not implement '" + typeof (IMappingSource).FullName + "': " + type, ex);
			}
			catch (Exception ex)
			{
				throw new ValidatorConfigurationException("Unable to instanciate a fluent validation definition: " + type, ex);
			}
		}

		[CLSCompliant(false)]
		public void AddClassDefinition<T>(IValidationDefinition<T> definition) where T : class
		{
			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}
			var ms = definition as IMappingSource;
			if(ms == null)
			{
				throw new ArgumentException("The argument is not an implementation of " + typeof(IMappingSource).FullName, "definition");
			}
			classMappings.Add(ms.GetMapping());
		}

		public IEnumerable<IClassMapping> GetMappings()
		{
			return classMappings;
		}
	}
}
