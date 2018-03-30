
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Just In Time Class Mapping Factory
	/// </summary>
	/// <remarks>
	/// The JITClassMappingFactory work outside the engine configuration.
	/// </remarks>
	internal class JITClassMappingFactory : IClassMappingFactory
	{
#if NETFX
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(JITClassMappingFactory));
#else
		private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(JITClassMappingFactory));
#endif

		#region IClassMappingFactory Members

		public IClassMapping GetClassMapping(System.Type clazz, ValidatorMode mode)
		{
			IClassMapping externalDefinition;
			IClassMapping result = null;
			switch (mode)
			{
				case ValidatorMode.UseAttribute:
					break;
				case ValidatorMode.UseExternal:
					result = GetExternalDefinitionFor(clazz);
					if (result == null)
					{
#if NETFX
						log.Warn(string.Format("External definition not foud for class {0} in ValidatorMode.UseExternal mode.", clazz.FullName));
#else
						Log.Warn("External definition not foud for class {0} in ValidatorMode.UseExternal mode.", clazz.FullName);
#endif
						return null; // <<<<<===
					}
					break;
				case ValidatorMode.OverrideAttributeWithExternal:
					externalDefinition = GetExternalDefinitionFor(clazz);
					if (externalDefinition != null)
					{
#if NETFX
						log.Debug("XmlOverAttribute applied for " + clazz.FullName);
#else
						Log.Debug("XmlOverAttribute applied for {0}", clazz.FullName);
#endif
						result = new XmlOverAttributeClassMapping(externalDefinition);
					}
					break;
				case ValidatorMode.OverrideExternalWithAttribute:
					externalDefinition = GetExternalDefinitionFor(clazz);
					if (externalDefinition != null)
					{
#if NETFX
						log.Debug("AttributeOverXml applied for " + clazz.FullName);
#else
						Log.Debug("AttributeOverXml applied for {0}", clazz.FullName);
#endif
						result = new AttributeOverXmlClassMapping(externalDefinition);
					}
					break;
			}
			if(result != null)
			{
				return result;
			}
			else
			{
#if NETFX
				log.Debug("Reflection applied for " + clazz.FullName);
#else
				Log.Debug("Reflection applied for {0}", clazz.FullName);
#endif
				return new ReflectionClassMapping(clazz);
			}
		}

		#endregion

		protected virtual IClassMapping GetExternalDefinitionFor(System.Type type)
		{
#if NETFX
			log.Debug("XML convention applied for " + type.FullName);
#else
			Log.Debug("XML convention applied for {0}", type.FullName);
#endif
			NhvMapping mapp = XmlMappingLoader.GetXmlMappingFor(type);
			if (mapp != null && mapp.@class.Length > 0)
			{
				return new XmlClassMapping(mapp.@class[0]);
			}

			return null;
		}
	}
}
