using System;
using log4net;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;

namespace NHibernate.Validator.Cfg
{
	public static class ValidatorInitializer
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorInitializer));

		public static void Initialize(Configuration cfg)
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

		public static void Initialize(Configuration cfg, ValidatorEngine ve)
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
				cfg.SetListener(ListenerType.PreInsert, new ValidatePreInsertEventListener());
				cfg.SetListener(ListenerType.PreUpdate, new ValidatePreUpdateEventListener());
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
				log.Warn("Unable to apply constraints on DDL for " + persistentClass.ClassName, ex);
			}
		}
	}
}