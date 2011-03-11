using NHibernate.Bytecode;
using NHibernate.Collection;
using NHibernate.Intercept;
using NHibernate.Proxy;

namespace NHibernate.Validator.Util
{
	/// <summary>
	/// Ported from NHibernate with changes
	/// </summary>
	public class NHibernateHelper
	{
		public static bool IsPropertyInitialized(object proxy, string propertyName)
		{
			object entity;

			if (!IsProxyFactoryConfigurated())
			{
				//if the proxy provider it's not configurated, can't be a proxy neither an instrumented field.
				return true;
			}

			if (proxy.IsProxy())
			{
				ILazyInitializer li = ((INHibernateProxy)proxy).HibernateLazyInitializer;
				if (li.IsUninitialized)
				{
					return false;
				}
				else
				{
					entity = li.GetImplementation();
				}
			}
			else
			{
				entity = proxy;
			}
			
			if (FieldInterceptionHelper.IsInstrumented(entity))
			{
				IFieldInterceptor interceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
				return interceptor == null || interceptor.IsInitializedField(propertyName);
			}
			else
			{
				return true;
			}
		}

		public static bool IsInitialized(object proxy)
		{
			var noProxyFactory = IsProxyFactoryConfigurated();
			
			if (noProxyFactory && proxy.IsProxy())
			{
				return !((INHibernateProxy)proxy).HibernateLazyInitializer.IsUninitialized;
			}
			else if (proxy is IPersistentCollection)
			{
				return ((IPersistentCollection)proxy).WasInitialized;
			}
			else
			{
				return true;
			}
		}

		public static bool IsProxyFactoryConfigurated()
		{
			try
			{
				var f = NHibernate.Cfg.Environment.BytecodeProvider.ProxyFactoryFactory;
				return true;
			}
			catch (ProxyFactoryFactoryNotConfiguredException)
			{
				return false;
			}
		}
	}
}