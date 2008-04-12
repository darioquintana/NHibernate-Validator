using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Before insert and update, executes the validator framework
	/// </summary>
	public class ValidateEventListener : IPreInsertEventListener, IPreUpdateEventListener, IInitializable
	{
		private bool isInitialized;
		// So far we are working with a private engine for persistence validation
		private readonly ValidatorEngine ve = new ValidatorEngine();

		private class SubElementsInspector : IValidatableSubElementsInspector
		{
			private readonly PersistentClass clazz;
			public SubElementsInspector(PersistentClass clazz)
			{
				this.clazz = clazz;
			}

			public void Inspect(ValidatableElement element)
			{
				AddSubElement(clazz.IdentifierProperty, element);

				foreach (Property property in clazz.PropertyIterator)
				{
					AddSubElement(property, element);
				}
			}

			private static void AddSubElement(Property property, ValidatableElement element)
			{
				if (property != null && property.IsComposite && !property.BackRef)
				{
					Component component = (Component)property.Value;
					if (component.IsEmbedded) return;

					IPropertyAccessor accesor = PropertyAccessorFactory.GetPropertyAccessor(property, EntityMode.Poco);

					IGetter getter = accesor.GetGetter(element.EntityType, property.Name);

					ClassValidator validator = new ClassValidator(getter.ReturnType);

					ValidatableElement subElement = new ValidatableElement(getter.ReturnType, validator, getter);

					foreach (Property currentProperty in component.PropertyIterator)
					{
						AddSubElement(currentProperty, subElement);
					}

					if (subElement.HasSubElements || subElement.Validator.HasValidationRules)
						element.AddSubElement(subElement);
				}
			}
		}

		#region IInitializable Members

		/// <summary>
		/// Initialize the validators, any non significant validators are not kept
		/// </summary>
		/// <param name="cfg"></param>
		public void Initialize(Configuration cfg)
		{
			if (isInitialized) return;

			ve.Configure(); // configure the private ValidatorEngine
			IEnumerable<PersistentClass> classes = cfg.ClassMappings;

			foreach (PersistentClass clazz in classes)
				ve.AddValidator(clazz.MappedClass, new SubElementsInspector(clazz));

			isInitialized = true;
		}

		#endregion


		#region IPreInsertEventListener Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public bool OnPreInsert(PreInsertEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion

		#region IPreUpdateEventListener Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="mode"></param>
		protected void Validate(object entity, EntityMode mode)
		{
			if (entity == null || !EntityMode.Poco.Equals(mode)) return;

			InvalidValue[] consolidatedInvalidValues = ve.Validate(entity);
			if (consolidatedInvalidValues.Length > 0)
				throw new InvalidStateException(consolidatedInvalidValues, entity.GetType().Name);
		}
	}
}