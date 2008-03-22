using System;
using log4net;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Util;
using NHibernate.Validator.Event;

namespace NHibernate.Validator.Cfg
{

	public class ValidatorInitializer
	{
		private ValidatorInitializer()
		{
		}

		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorInitializer));

		public static void Initialize(Configuration cfg)
		{
			bool ApplyToDDL = PropertiesHelper.GetBoolean(Environment.ApplyToDDL, cfg.Properties, true);
			bool AutoRegisterListeners = PropertiesHelper.GetBoolean(Environment.AutoregisterListeners, cfg.Properties, true);
			
			//Apply To DDL
			if (ApplyToDDL)
			{
				foreach(PersistentClass persistentClazz in cfg.ClassMappings)
				{
					try
					{
						ClassValidator classValidator = new ClassValidator(persistentClazz.MappedClass);
						classValidator.Apply(persistentClazz);
					}
					catch(Exception ex)
					{
						log.Warn("Unable to apply constraints on DDL for " + persistentClazz.ClassName, ex);
					}
				}
			}

			//Autoregister Listeners
			if(AutoRegisterListeners)
			{
				cfg.SetListener(ListenerType.PreInsert, new ValidatePreInsertEventListener());
				cfg.SetListener(ListenerType.PreUpdate, new ValidatePreUpdateEventListener());
			}
		}
	}
}