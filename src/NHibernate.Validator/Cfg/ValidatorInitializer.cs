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
			ValidatorEngine ve = new ValidatorEngine();
			ve.Configure();
			bool applyToDDL = ve.ApplyToDDL;
			bool autoRegisterListeners = ve.AutoRegisterListeners;
			ValidatorMode vm = ve.DefaultMode;
			log.Info("Using validation mode = " + vm);
			Initialize(cfg, applyToDDL, autoRegisterListeners, vm);
		}

		public static void Initialize(Configuration cfg, bool applyToDDL, bool autoRegisterListeners, ValidatorMode validatorMode)
		{
			//Apply To DDL
			if (applyToDDL)
			{
				foreach (PersistentClass persistentClazz in cfg.ClassMappings)
				{
					ApplyValidatorToDDL(persistentClazz, validatorMode);
				}
			}

			//Autoregister Listeners
			if (autoRegisterListeners)
			{
				cfg.SetListener(ListenerType.PreInsert, new ValidatePreInsertEventListener());
				cfg.SetListener(ListenerType.PreUpdate, new ValidatePreUpdateEventListener());
			}
		}

		private static void ApplyValidatorToDDL(PersistentClass persistentClass, ValidatorMode validatorMode)
		{
			try
			{
				ClassValidator classValidator = new ClassValidator(persistentClass.MappedClass, validatorMode);
				classValidator.Apply(persistentClass);
			}
			catch (Exception ex)
			{
				log.Warn("Unable to apply constraints on DDL for " + persistentClass.ClassName, ex);
			}
		}
	}
}