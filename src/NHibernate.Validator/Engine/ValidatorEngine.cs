using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Resources;
using System.Xml;
using log4net;
using NHibernate.Mapping;
using NHibernate.Util;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// The engine of NHibernate Validator
	/// </summary>
	/// <remarks>
	/// The engine is the easy way to work with NHibernate.Validator.
	/// It hold all class validators.
	/// Usually an application will create a single <see cref="ValidatorEngine" />.
	/// </para>
	/// </remarks>
	public class ValidatorEngine
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorEngine));

		// TODO : make the class thread-safe and may be serializable
		private StateFullClassValidatorFactory factory;
		private IMessageInterpolator interpolator;
		private ValidatorMode defaultMode;
		private bool applyToDDL;
		private bool autoRegisterListeners;
		private readonly Dictionary<System.Type, ValidatableElement> validators = new Dictionary<System.Type, ValidatableElement>();
		private static readonly ValidatableElement alwaysValidPlaceHolder = new ValidatableElement(typeof (object), new EmptyClassValidator());

		private class EmptyClassValidator: IClassValidator
		{
			public bool HasValidationRules
			{
				get { return false; }
			}

			public InvalidValue[] GetInvalidValues(object bean)
			{
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;
			}

			public InvalidValue[] GetInvalidValues(object bean, string propertyName)
			{
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;
			}

			public void AssertValid(object bean)
			{
			}

			public InvalidValue[] GetPotentialInvalidValues(string propertyName, object value)
			{
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;
			}

			public void Apply(PersistentClass persistentClass)
			{
			}
		}

		public ValidatorEngine()
		{
			factory = new StateFullClassValidatorFactory(null, null, null, ValidatorMode.UseAttribute);
		}

		/// <summary>
		/// Default MessageInterpolator
		/// </summary>
		public IMessageInterpolator Interpolator
		{
			get { return interpolator; }
		}

		/// <summary>
		/// Default Mode to construct validators
		/// </summary>
		public ValidatorMode DefaultMode
		{
			get { return defaultMode; }
		}

		/// <summary>
		/// Database schema-level validation enabled
		/// </summary>
		public bool ApplyToDDL
		{
			get { return applyToDDL; }
		}

		/// <summary>
		/// NHibernate event-based validation
		/// </summary>
		public bool AutoRegisterListeners
		{
			get { return autoRegisterListeners; }
		}

		/// <summary>
		/// Configure NHibernate.Validator using the <c>&lt;nhv-configuration&gt;</c> section
		/// from the application config file, if found, or the file <c>nhvalidator.cfg.xml</c> if the
		/// <c>&lt;nhv-configuration&gt;</c> section not include the session-factory configuration.
		/// </summary>
		/// <remarks>
		/// To configure NHibernate explicitly using <c>nhvalidator.cfg.xml</c>, appling merge/override
		/// of the application configuration file, use this code:
		/// <code>
		///		configuration.Configure("path/to/nhvalidator.cfg.xml");
		/// </code>
		/// </remarks>
		public void Configure()
		{
			INHVConfiguration nhvhc = ConfigurationManager.GetSection(CfgXmlHelper.CfgSectionName) as INHVConfiguration;
			if (nhvhc != null)
			{
				Configure(nhvhc);
			}
			else
			{
				Configure(GetDefaultConfigurationFilePath());
			}
		}

		/// <summary>
		/// Configure NHibernate.Validator using the file specified.
		/// </summary>
		/// <param name="configFilePath">The location of the XML file to use to configure NHibernate.Validator.</param>
		/// <remarks>
		/// Calling Configure(string) will override/merge the values set in app.config or web.config
		/// </remarks>
		public void Configure(string configFilePath)
		{
			using (XmlTextReader reader = new XmlTextReader(configFilePath))
			{
				Configure(reader);
			}
		}

		/// <summary>
		/// Configure NHibernate.Validator using the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="configReader">The <see cref="XmlReader"/> that contains the Xml to configure NHibernate.Validator.</param>
		/// <remarks>
		/// Calling Configure(XmlReader) will overwrite the values set in app.config or web.config
		/// </remarks>
		public void Configure(XmlReader configReader)
		{
			if (configReader == null)
			{
				throw new ValidatorConfigurationException("Could not configure NHibernate.Validator.",
				                                          new ArgumentNullException("configReader"));
			}

			INHVConfiguration nhvc = new NHVConfiguration(configReader);
			Configure(nhvc);
		}

		/// <summary>
		/// Configure NHibernate.Validator using the specified <see cref="INHVConfiguration"/>.
		/// </summary>
		/// <param name="config">The <see cref="INHVConfiguration"/> that is the configuration reader to configure NHibernate.Validator.</param>
		/// <remarks>
		/// Calling Configure(INHVConfiguration) will overwrite the values set in app.config or web.config
		/// </remarks>
		public void Configure(INHVConfiguration config)
		{
			if (config == null)
			{
				throw new ValidatorConfigurationException("Could not configure NHibernate.Validator.",
				                                          new ArgumentNullException("config"));
			}

			Clear();

			applyToDDL = PropertiesHelper.GetBoolean(Environment.ApplyToDDL, config.Properties, true);
			autoRegisterListeners = PropertiesHelper.GetBoolean(Environment.AutoregisterListeners, config.Properties, true);
			defaultMode = CfgXmlHelper.ValidatorModeConvertFrom(PropertiesHelper.GetString(Environment.ValidatorMode, config.Properties, string.Empty));
			interpolator = GetInterpolator(PropertiesHelper.GetString(Environment.MessageInterpolatorClass, config.Properties, string.Empty));

			factory = new StateFullClassValidatorFactory(null, null, interpolator, defaultMode);

			// UpLoad Mappings
			MappingLoader ml = new MappingLoader();
			ml.LoadMappings(config.Mappings);
			Initialize(ml);
		}

		private void Initialize(MappingLoader loader)
		{
			factory.Initialize(loader);
			foreach (KeyValuePair<System.Type, IClassValidator> validator in factory.Validators)
			{
				AddValidatableElement(new ValidatableElement(validator.Key, validator.Value));
			}
		}

		private void Clear()
		{
			validators.Clear();
		}

		/// <summary>
		/// Apply constraints/rules on a entity instance.
		/// </summary>
		/// <param name="entity">The entity instance to validate</param>
		/// <returns>All the failures or an empty array if <paramref name="entity"/> is null.</returns>
		/// <remarks>
		/// If the <see cref="System.Type"/> of the <paramref name="entity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] Validate(object entity)
		{
			if (entity == null) 
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			ValidatableElement element = GetElementOrNew(entity.GetType());

			List<InvalidValue> result = new List<InvalidValue>();
			ValidateSubElements(element, entity, result);
			result.AddRange(element.Validator.GetInvalidValues(entity));
			return result.ToArray();
		}

		private static void ValidateSubElements(ValidatableElement element, object entity, List<InvalidValue> consolidatedInvalidValues)
		{
			if (element != null)
			{
				foreach (ValidatableElement subElement in element.SubElements)
				{
					object component = subElement.Getter.Get(entity);
					consolidatedInvalidValues.AddRange(subElement.Validator.GetInvalidValues(component));
					ValidateSubElements(subElement, component, consolidatedInvalidValues);
				}
			}
		}

		/// <summary>
		/// Apply constraints/rules on a entity instance.
		/// </summary>
		/// <param name="entity">The entity instance to validate</param>
		/// <returns>
		/// False if there is one or more the failures; True otherwise (including when <paramref name="entity"/> is null).
		/// </returns>
		/// <remarks>
		/// If the <see cref="System.Type"/> of the <paramref name="entity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public bool IsValid(object entity)
		{
			// TODO: improving breaking at the first invalidValue
			return Validate(entity).Length == 0;
		}

		/// <summary>
		/// Assert a valid entity.
		/// </summary>
		/// <param name="entity">The entity instance to validate</param>
		/// <exception cref="InvalidStateException">when <paramref name="entity"/> have an invalid state.</exception>
		/// <remarks>
		/// If the <see cref="System.Type"/> of the <paramref name="entity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public void AssertValid(object entity)
		{
			if (entity == null) return;
			ValidatableElement element = GetElementOrNew(entity.GetType());
			element.Validator.AssertValid(entity);
		}

		/// <summary>
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given <see cref="System.Type"/>.
		/// </summary>
		/// <typeparam name="T">The entity type</typeparam>
		/// <param name="propertyName">The name of a property</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>All the invalid values.</returns>
		/// <remarks>
		/// If the <typeparamref name="T"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] ValidatePropertyValue<T>(string propertyName, object value)
		{
			return ValidatePropertyValue(typeof(T), propertyName, value);
		}

		/// <summary>
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given entity instance.
		/// </summary>
		/// <param name="entity">The entity instance to validate</param>
		/// <param name="propertyName">The name of a property</param>
		/// <returns>All the invalid values.</returns>
		/// <remarks>
		/// If the <see cref="System.Type"/> of the <paramref name="entity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] ValidatePropertyValue(object entity, string propertyName)
		{
			if (entity == null)
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			ValidatableElement element = GetElementOrNew(entity.GetType());

			return element.Validator.GetInvalidValues(entity, propertyName);
		}

		/// <summary>
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given entity instance.
		/// </summary>
		/// <param name="entityType">The entity instance to validate</param>
		/// <param name="propertyName">The name of a property</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>All the invalid values.</returns>
		public InvalidValue[] ValidatePropertyValue(System.Type entityType, string propertyName, object value)
		{
			IClassValidator cv = GetElementOrNew(entityType).Validator;
			return cv.GetPotentialInvalidValues(propertyName, value);
		}

		/// <summary>
		/// Add a validator to the engine or override existing one.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <remarks>
		/// Create an istance of <see cref="IClassValidator"/> for the given <typeparamref name="T"/>.
		/// </remarks>
		public void AddValidator<T>()
		{
			AddValidator<T>(null);
		}

		/// <summary>
		/// Add a validator to the engine or override existing one.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <param name="inspector">Inspector for sub-elements</param>
		public void AddValidator<T>(IValidatableSubElementsInspector inspector)
		{
			AddValidator(typeof (T), inspector);
		}

		internal void AddValidator(System.Type entityType, IValidatableSubElementsInspector inspector)
		{
			IClassValidator cv = GetNewClassValidator(entityType, null);
			ValidatableElement element = new ValidatableElement(entityType, cv);
			if (inspector != null)
				inspector.Inspect(element);
			AddValidatableElement(element);
		}

		internal void AddValidatableElement(ValidatableElement element)
		{
			if (element.HasSubElements || element.Validator.HasValidationRules)
				validators[element.EntityType] = element;
			else
				validators[element.EntityType] = alwaysValidPlaceHolder;
		}

		/// <summary>
		/// Get a knowed <see cref="IClassValidator"/>.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <returns>A knowed <see cref="IClassValidator"/> for the give type 
		/// or null if the the <typeparamref name="T"/> was never used in the engine instance.</returns>
		public IClassValidator GetValidator<T>()
		{
			return GetValidator(typeof(T));
		}

		internal ValidatableElement GetElementOrNew(System.Type entityType)
		{
			ValidatableElement element;
			if (!validators.TryGetValue(entityType, out element))
			{
				IClassValidator cv = GetNewClassValidator(entityType, null);
				element = new ValidatableElement(entityType, cv);
				AddValidatableElement(element);
			}
			return element;
		}

		private IClassValidator GetValidator(System.Type entityType)
		{
			ValidatableElement element;
			validators.TryGetValue(entityType, out element);

			return element == null ? null : element.Validator;
		}

		private IClassValidator GetNewClassValidator(System.Type entityType, ResourceManager resource)
		{
			return factory.GetRootValidator(entityType);
		}

		private IMessageInterpolator GetInterpolator(string interpolatorString)
		{
			if (!string.IsNullOrEmpty(interpolatorString))
			{
				try
				{
					System.Type interpolatorType = ReflectHelper.ClassForName(interpolatorString);
					interpolator = (IMessageInterpolator)Activator.CreateInstance(interpolatorType);
				}
				catch (MissingMethodException ex)
				{
					throw new ValidatorConfigurationException("Public constructor was not found at message interpolator: " + interpolatorString, ex);
				}
				catch (InvalidCastException ex)
				{
					throw new ValidatorConfigurationException(
						"Type does not implement the interface " + typeof(IMessageInterpolator).GetType().Name + ": " + interpolatorString,
						ex);
				}
				catch (Exception ex)
				{
					throw new ValidatorConfigurationException("Unable to instanciate message interpolator: " + interpolatorString, ex);
				}
			}
			return interpolator;
		}

		private static string GetDefaultConfigurationFilePath()
		{
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
			string binPath = relativeSearchPath == null ? baseDir : Path.Combine(baseDir, relativeSearchPath);
			return Path.Combine(binPath, CfgXmlHelper.DefaultNHVCfgFileName);
		}
	}
}
