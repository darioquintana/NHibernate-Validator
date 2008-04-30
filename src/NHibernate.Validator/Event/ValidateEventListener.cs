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
		private static readonly object padlock = new object();
		private bool isInitialized;
		private static ValidatorEngine ve; // engine for listeners

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

					IClassValidator validator =  ve.GetClassValidator(getter.ReturnType);

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
			if(Validator.Cfg.Environment.SharedEngineProvider != null)
			{
				ve = Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
			}
			else
			{
				// thread safe lazy initialization of local engine
				lock (padlock)
				{
					if (ve == null)
					{
						ve = new ValidatorEngine();
						ve.Configure(); // configure the private ValidatorEngine
					}
				}
			}

			IEnumerable<PersistentClass> classes = cfg.ClassMappings;

			foreach (PersistentClass clazz in classes)
				ve.AddValidator(clazz.MappedClass, new SubElementsInspector(clazz));

			isInitialized = true;
		}

		#endregion


		#region IPreInsertEventListener Members

		public bool OnPreInsert(PreInsertEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion

		#region IPreUpdateEventListener Members

		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			Validate(@event.Entity, @event.Source.EntityMode);
			return false;
		}

		#endregion


		protected static void Validate(object entity, EntityMode mode)
		{
			if (entity == null || !EntityMode.Poco.Equals(mode)) return;

			InvalidValue[] consolidatedInvalidValues = ve.Validate(entity);
			if (consolidatedInvalidValues.Length > 0)
				throw new InvalidStateException(consolidatedInvalidValues, entity.GetType().Name);
		}
	}
}