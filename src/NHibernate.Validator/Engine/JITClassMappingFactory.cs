using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Engine
{
	internal class JITClassMappingFactory : IClassMappingFactory
	{
		#region IClassMappingFactory Members

		public IClassMapping GetClassMapping(System.Type clazz, ValidatorMode mode)
		{
			NhvmClass nhvm;
			switch (mode)
			{
				case ValidatorMode.UseAttribute:
					return new ReflectionClassMapping(clazz);
				case ValidatorMode.UseXml:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm == null)
					{
						return null;
					}
					return new XmlClassMapping(nhvm);
				case ValidatorMode.OverrideAttributeWithXml:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm == null)
					{
						return new ReflectionClassMapping(clazz);
					}
					else
					{
						return new XmlOverAttributeClassMapping(nhvm);
					}
				case ValidatorMode.OverrideXmlWithAttribute:
					nhvm = GetXmlDefinitionFor(clazz);
					if (nhvm == null)
					{
						return new ReflectionClassMapping(clazz);
					}
					else
					{
						return new AttributeOverXmlClassMapping(nhvm);
					}
			}
			return null;
		}

		#endregion

		protected virtual NhvmClass GetXmlDefinitionFor(System.Type type)
		{
			NhvMapping mapp = MappingLoader.GetMappingFor(type);
			if (mapp != null && mapp.@class.Length > 0)
			{
				return mapp.@class[0];
			}

			return null;
		}
	}
}