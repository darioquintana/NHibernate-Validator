using log4net;
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
		private static readonly ILog log = LogManager.GetLogger(typeof(ClassValidator));

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
						log.Warn(string.Format("External definition not foud for class {0} in ValidatorMode.UseExternal mode.", clazz.FullName));
						return null; // <<<<<===
					}
					break;
				case ValidatorMode.OverrideAttributeWithExternal:
					externalDefinition = GetExternalDefinitionFor(clazz);
					if (externalDefinition != null)
					{
						result = new XmlOverAttributeClassMapping(externalDefinition);
					}
					break;
				case ValidatorMode.OverrideExternalWithAttribute:
					externalDefinition = GetExternalDefinitionFor(clazz);
					if (externalDefinition != null)
					{
						result = new AttributeOverXmlClassMapping(externalDefinition);
					}
					break;
			}
			return result ?? new ReflectionClassMapping(clazz);
		}

		#endregion

		protected virtual IClassMapping GetExternalDefinitionFor(System.Type type)
		{
			NhvMapping mapp = MappingLoader.GetXmlMappingFor(type);
			if (mapp != null && mapp.@class.Length > 0)
			{
				return new XmlClassMapping(mapp.@class[0]);
			}

			return null;
		}
	}
}