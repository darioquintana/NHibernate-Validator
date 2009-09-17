using NHibernate.Proxy;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// An <see cref="IEntityTypeInspector"/> for proxy coming from NHibernate.
	/// </summary>
	public class DefaultEntityTypeInspector: IEntityTypeInspector
	{
		#region Implementation of IEntityTypeInspector

		public virtual System.Type GuessType(object entityInstance)
		{
			var proxy = entityInstance as INHibernateProxy;
			if (proxy != null)
			{
				return proxy.HibernateLazyInitializer.PersistentClass;
			}
			return null;
		}

		#endregion
	}
}