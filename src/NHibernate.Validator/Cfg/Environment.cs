using System;
using System.Collections.Generic;
using System.Configuration;

using NHibernate.Util;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// NHibernate Validator properties
	/// </summary>
	public static class Environment
	{
		/// <summary>
		/// Apply DDL changes on Hibernate metamodel when using validator with Hibernate Annotations.
		/// Default to true.
		/// </summary>
		public const string ApplyToDDL = "apply_to_ddl";

		/// <summary>
		/// Auto generate validators from NHib mapping if there are no compatible validators implicitly defined
		/// </summary>
		public const string AutoGenerateFromMapping = "auto_generate_from_mapping";

		/// <summary>
		/// Enable listeners auto registration in Hibernate Annotations and EntityManager.
		/// Default to true.
		/// </summary>
		public const string AutoregisterListeners = "autoregister_listeners";

		/// <summary>
		/// Message interpolator class used. The same instance is shared across all ClassValidators 
		/// </summary>
		public const string MessageInterpolatorClass = "message_interpolator_class";

		/// <summary>
		/// Mapping loader implementor.
		/// </summary>
		public const string MappingLoaderClass = "external_mappings_loader_class";

		/// <summary>
		/// Constraint Validator Factory used to create Validators.
		/// </summary>
		public const string ConstraintValidatorFactoryClass = "constraint_validator_factory";

		/// <summary>
		/// Define validation mode.
		/// Default <see cref="Engine.ValidatorMode.UseAttribute"/>
		/// </summary>
		/// <remarks>
		/// Allowed values are available in <see cref="Engine.ValidatorMode"/>.
		/// </remarks>
		/// <seealso cref="Engine.ValidatorMode"/>
		public const string ValidatorMode = "default_validator_mode";

		/// <summary>
		/// Base name of resource file for embedded validators.
		/// </summary>
		public const string BaseNameOfMessageResource = "NHibernate.Validator.Properties.DefaultValidatorMessages";

		/// <summary>
		/// The configuration property name for the custom resource manager
		/// </summary>
		public const string CustomResourceManager = "resource_manager";

		/// <summary>
		/// Define the provider of shared engine between various logical layers.
		/// </summary>
		/// <remarks>
		/// The main target of shared engine is have only one engine for the NHibernate events of validator.
		/// If an application configure a certain shared engine provider and use it to get the ValidatorEngine
		/// the result is that the events for NH configure the ValidatorEngine and the application can
		/// share the same engine ensuring that app and NH-event make the same validation.
		/// <para/>
		/// The configuration of shared_engine_provider is ignored outside app/web config file.
		/// </remarks>
		public const string SharedEngineClass = "shared_engine_provider";

#if NETFX
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(Environment));
#else
		private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(Environment));
#endif

		private static readonly Dictionary<string, string> GlobalProperties;

		private static ISharedEngineProvider sharedEngineProviderInstance;

		static Environment()
		{

			GlobalProperties = new Dictionary<string, string>();
			LoadGlobalPropertiesFromAppConfig();

			sharedEngineProviderInstance = BuildSharedEngine(GlobalProperties);

			if (sharedEngineProviderInstance != null)
			{
#if NETFX
				log.Info("Using shared engine provider:" + sharedEngineProviderInstance.GetType().AssemblyQualifiedName);
#else
				Log.Info("Using shared engine provider: {0}", sharedEngineProviderInstance.GetType().AssemblyQualifiedName);
#endif
			}
		}

		private static void LoadGlobalPropertiesFromAppConfig()
		{
			object config = ConfigurationManager.GetSection(CfgXmlHelper.CfgSectionName);

			INHVConfiguration NHVconfig = config as INHVConfiguration;
			if (NHVconfig != null)
			{
				if (!string.IsNullOrEmpty(NHVconfig.SharedEngineProviderClass))
					GlobalProperties[SharedEngineClass] = NHVconfig.SharedEngineProviderClass;
			}
		}

		private static ISharedEngineProvider BuildSharedEngine(IDictionary<string, string> properties)
		{
			string clazz;
			ISharedEngineProvider result = null;
			if (properties.TryGetValue(SharedEngineClass, out clazz))
			{
				result = (ISharedEngineProvider) Activator.CreateInstance(ReflectHelper.ClassForFullName(clazz));
			}
			return result;
		}

		/// <summary>
		/// Shared engine provider instance.
		/// </summary>
		/// <remarks>
		/// The Shared engine instance can be injected at run-time even if if not the better way to work with it.
		/// </remarks>
		public static ISharedEngineProvider SharedEngineProvider
		{
			get { return sharedEngineProviderInstance; }
			set { sharedEngineProviderInstance = value; }
		}

		public static IConstraintValidatorFactory ConstraintValidatorFactory { get; set; }
	}
}
