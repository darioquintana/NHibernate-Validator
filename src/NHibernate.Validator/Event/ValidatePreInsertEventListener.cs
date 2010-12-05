using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using Environment=NHibernate.Validator.Cfg.Environment;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Before insert, executes the validator framework
	/// </summary>
	/// <remarks>
	/// Because, usually, we validate on insert and on update we are
	/// using the same environment for PreInsert and  PreUpdate event listeners,
	/// the initialization of the environment (the ValidatorEngine) was or will be done in 
	/// ValidatePreInsertEventListener by NH.
	/// This give us better performance on NH startup.
	/// </remarks>
	[Serializable]
	public class ValidatePreInsertEventListener : ValidateEventListener, IPreInsertEventListener, IInitializable
	{
		private bool isInitialized;

		#region IInitializable Members

		/// <summary>
		/// Initialize the validators, any non significant validators are not kept
		/// </summary>
		/// <param name="cfg"></param>
		public void Initialize(Configuration cfg)
		{
			if (isInitialized || cfg == null)
			{
				return;
			}
			Engine = null;
			if (Environment.SharedEngineProvider != null)
			{
				Engine = Environment.SharedEngineProvider.GetEngine();
			}

			IEnumerable<PersistentClass> classes = cfg.ClassMappings;

			foreach (PersistentClass clazz in classes)
			{
				Engine.AddValidator(clazz.MappedClass, new SubElementsInspector(clazz));
			}

			isInitialized = true;
		}

		#endregion

		#region IPreInsertEventListener Members

		public bool OnPreInsert(PreInsertEvent @event)
		{
			Validate(@event.Entity, @event.Session.EntityMode);
			return false;
		}

		#endregion

		#region Nested type: SubElementsInspector

		private class SubElementsInspector : IValidatableSubElementsInspector
		{
			private readonly PersistentClass clazz;

			public SubElementsInspector(PersistentClass clazz)
			{
				this.clazz = clazz;
			}

			#region IValidatableSubElementsInspector Members

			public void Inspect(ValidatableElement element)
			{
				AddSubElement(clazz.IdentifierProperty, element);

				foreach (Property property in clazz.PropertyIterator)
				{
					AddSubElement(property, element);
				}
			}

			#endregion

			private static void AddSubElement(Property property, ValidatableElement element)
			{
				if (property != null && property.IsComposite && !property.BackRef)
				{
					Component component = (Component) property.Value;
					if (component.IsEmbedded)
					{
						return;
					}

					if (property.PersistentClass != null)
					{
						var cv = Engine.GetClassValidator(property.PersistentClass.MappedClass);

						if (cv != null)
						{
							if (cv.GetMemberConstraints(property.Name).OfType<ValidAttribute>().Any())
							{
								// the components is already marked as Valid
								return;
							}
						}
					}

					IPropertyAccessor accesor = PropertyAccessorFactory.GetPropertyAccessor(property, EntityMode.Poco);

					IGetter getter = accesor.GetGetter(element.EntityType, property.Name);

					IClassValidator validator = Engine.GetClassValidator(getter.ReturnType);
					if (validator != null)
					{
						ValidatableElement subElement = new ValidatableElement(getter.ReturnType, validator, getter);

						foreach (Property currentProperty in component.PropertyIterator)
						{
							AddSubElement(currentProperty, subElement);
						}

						if (subElement.HasSubElements || subElement.Validator.HasValidationRules)
						{
							element.AddSubElement(subElement);
						}
					}
				}
			}
		}

		#endregion
	}
}