using System;
using System.Linq;
using log4net;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Extensions to integrate NHibernate.Validator with NHibernate
	/// </summary>
	public static class ValidatorInitializer
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorInitializer));

		/// <summary>
		/// Initialize NHibernate's events and/or DLL.
		/// </summary>
		/// <param name="cfg">The NHibernate.Cfg.Configuration before build the session factory.</param>
		/// <remarks>
		/// If the <see cref="ISharedEngineProvider"/> was configured or the
		/// <see cref="Environment.SharedEngineProvider"/> was set, it will be used for the integration;
		/// otherwise a new <see cref="ValidatorEngine"/> will be configured and used.
		/// <para>
		/// To have DDL-integration you must set the configuration property "apply_to_ddl" to true
		/// </para>
		/// <para>
		/// To have events-integration you must set the configuration property "autoregister_listeners" to true
		/// </para>
		/// </remarks>
		public static void Initialize(this Configuration cfg)
		{
			if (cfg == null)
				throw new ArgumentNullException("cfg");
			ValidatorEngine ve;
			if (Environment.SharedEngineProvider != null)
			{
				ve = Environment.SharedEngineProvider.GetEngine();
			}
			else
			{
				ve = new ValidatorEngine();
				ve.Configure();
			}
			Initialize(cfg, ve);
		}

		/// <summary>
		/// Initialize NHibernate's events and/or DLL.
		/// </summary>
		/// <param name="cfg">The NHibernate.Cfg.Configuration before build the session factory.</param>
		/// <param name="ve">A configured ValidatorEngine (after call <see cref="ValidatorEngine.Configure()"/>)</param>
		/// <remarks>
		/// <para>
		/// To have DDL-integration you must set the configuration property "apply_to_ddl" to true
		/// </para>
		/// <para>
		/// To have events-integration you must set the configuration property "autoregister_listeners" to true
		/// </para>
		/// </remarks>
		public static void Initialize(this Configuration cfg, ValidatorEngine ve)
		{
			//Apply To DDL
			if (ve.ApplyToDDL)
			{
				foreach (PersistentClass persistentClazz in cfg.ClassMappings)
				{
					ApplyValidatorToDDL(persistentClazz, ve);
				}
			}

			//Autoregister Listeners
			if (ve.AutoRegisterListeners)
			{
				cfg.SetListeners(ListenerType.PreInsert,
				                 cfg.EventListeners.PreInsertEventListeners.Concat(new[] {new ValidatePreInsertEventListener()}).ToArray());
				cfg.SetListeners(ListenerType.PreUpdate,
												 cfg.EventListeners.PreUpdateEventListeners.Concat(new[] { new ValidatePreUpdateEventListener() }).ToArray());
			}
		}

		private static void ApplyValidatorToDDL(PersistentClass persistentClass, ValidatorEngine ve)
		{
			try
			{
				IClassValidator classValidator = ve.GetClassValidator(persistentClass.MappedClass);
				classValidator.Apply(persistentClass);
			}
			catch (Exception ex)
			{
				log.Warn(
					string.Format("Unable to apply constraints on DDL for [MappedClass={0}]", persistentClass.MappedClass.FullName), ex);
			}
		}
	}
}