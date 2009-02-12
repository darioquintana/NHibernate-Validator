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
			NhvmClass nhvm;
			IClassMapping result = null;
			switch (mode)
			{
				case ValidatorMode.UseAttribute:
					break;
				case ValidatorMode.UseXml:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm == null)
					{
						log.Warn(string.Format("XML definition not foud for class {0} in ValidatorMode.UseXml mode.", clazz.FullName));
						return null; // <<<<<===
					}
					result = new XmlClassMapping(nhvm);
					break;
				case ValidatorMode.OverrideAttributeWithXml:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm != null)
					{
						result = new XmlOverAttributeClassMapping(nhvm);
					}
					break;
				case ValidatorMode.OverrideXmlWithAttribute:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm != null)
					{
						result = new AttributeOverXmlClassMapping(nhvm);
					}
					break;
			}
			return result ?? new ReflectionClassMapping(clazz);
		}

		#endregion

		protected virtual NhvmClass GetXmlDefinitionFor(System.Type type)
		{
			NhvMapping mapp = MappingLoader.GetXmlMappingFor(type);
			if (mapp != null && mapp.@class.Length > 0)
			{
				return mapp.@class[0];
			}

			return null;
		}
	}
}