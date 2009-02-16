using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq.Expressions;
using System.Xml;
using log4net;
using NHibernate.Mapping;
using NHibernate.Util;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Util;
using Environment=NHibernate.Validator.Cfg.Environment;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// The engine of NHibernate Validator
	/// </summary>
	/// <remarks>
	/// The engine is the easy way to work with NHibernate.Validator.
	/// It hold all class validators.
	/// Usually an application will create a single <see cref="ValidatorEngine" />.
	/// </remarks>
	/// <seealso cref="Cfg.Environment"/>
	/// <seealso cref="ISharedEngineProvider"/>.
	[Serializable]
	public class ValidatorEngine
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ValidatorEngine));

		private StateFullClassValidatorFactory factory;
		private IMessageInterpolator interpolator;
		private ValidatorMode defaultMode;
		private bool applyToDDL;
		private bool autoRegisterListeners;

		private readonly ThreadSafeDictionary<System.Type, ValidatableElement> validators =
			new ThreadSafeDictionary<System.Type, ValidatableElement>(new Dictionary<System.Type, ValidatableElement>());
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
		/// If both are found merge the two configuration.
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
			var nhvhc = ConfigurationManager.GetSection(CfgXmlHelper.CfgSectionName) as INHVConfiguration;
			if (nhvhc != null)
			{
				Configure(nhvhc);
			}
			string filePath = GetDefaultConfigurationFilePath();
			if (File.Exists(filePath))
				Configure(filePath); // merge the configuration
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
			using (var reader = new XmlTextReader(configFilePath))
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

			INHVConfiguration nhvc = new XmlConfiguration(configReader);
			Configure(nhvc);
		}

		/// <summary>
		/// Configure NHibernate.Validator using the specified <see cref="INHVConfiguration"/>.
		/// </summary>
		/// <param name="config">The <see cref="INHVConfiguration"/> that is the configuration reader to configure NHibernate.Validator.</param>
		/// <remarks>
		/// Calling Configure(INHVConfiguration) will overwrite the values set in app.config or web.config.
		/// <para>
		/// You can use this overload is you are working with Attributes or Xml-files.
		/// </para>
		/// </remarks>
		public void Configure(INHVConfiguration config)
		{
			Configure(config, config as IMappingLoader);
		}

		/// <summary>
		/// Configure NHibernate.Validator using the specified <see cref="INHVConfiguration"/>.
		/// </summary>
		/// <param name="config">The <see cref="INHVConfiguration"/> that is the configuration reader to configure NHibernate.Validator.</param>
		/// <param name="mappingLoader">The <see cref="XmlMappingLoader"/> instance.</param>
		/// <remarks>
		/// Calling Configure(INHVConfiguration) will overwrite the values set in app.config or web.config
		/// </remarks>
		public void Configure(INHVConfiguration config, IMappingLoader mappingLoader)
		{
			if (config == null)
			{
				throw new ValidatorConfigurationException("Could not configure NHibernate.Validator.",
				                                          new ArgumentNullException("config"));
			}

			Clear();

			applyToDDL = PropertiesHelper.GetBoolean(Environment.ApplyToDDL, config.Properties, true);
			autoRegisterListeners = PropertiesHelper.GetBoolean(Environment.AutoregisterListeners, config.Properties, true);
			defaultMode =
				CfgXmlHelper.ValidatorModeConvertFrom(PropertiesHelper.GetString(Environment.ValidatorMode, config.Properties,
				                                                                 string.Empty));
			interpolator =
				GetImplementation<IMessageInterpolator>(
					PropertiesHelper.GetString(Environment.MessageInterpolatorClass, config.Properties, string.Empty),
					"message interpolator");

			factory = new StateFullClassValidatorFactory(null, null, interpolator, defaultMode);

			// UpLoad Mappings
			if(mappingLoader == null)
			{
				// Configured or Default loader (XmlMappingLoader)
				mappingLoader = GetImplementation<IMappingLoader>(
				                	PropertiesHelper.GetString(Environment.MappingLoaderClass, config.Properties, string.Empty),
				                	"mapping loader") ?? new XmlMappingLoader();
			}
			mappingLoader.LoadMappings(config.Mappings);
			Initialize(mappingLoader);
		}

		private void Initialize(IMappingLoader loader)
		{
			factory.Initialize(loader);
			foreach (KeyValuePair<System.Type, IClassValidator> validator in factory.Validators)
			{
				AddValidatableElement(new ValidatableElement(validator.Key, validator.Value));
			}
		}

		public void Clear()
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

			System.Type entityType = entity.GetType();

			if (!ClassValidator.ShouldNeedValidation(entityType))
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			ValidatableElement element = GetElementOrNew(entityType);

			var result = new List<InvalidValue>();
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
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given <see cref="System.Type"/>.
		/// </summary>
		/// <typeparam name="TEntity">The entity type</typeparam>
		/// <typeparam name="TProperty">The property type.</typeparam>
		/// <param name="expression">The lamda expression of the property getter.</param>
		/// <param name="value">The potencial value of the property.</param>
		/// <remarks>
		/// If the <typeparamref name="TEntity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] ValidatePropertyValue<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression, TProperty value) where TEntity : class
		{
			var propertyName = TypeUtils.DecodeMemberAccessExpression(expression).Name;
			return ValidatePropertyValue(typeof (TEntity), propertyName, value);
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
		[Obsolete("Use ValidatePropertyValue<TEntity, TProperty>(TEntity, Expression<Func<TEntity, TProperty>>) instead.")]
		public InvalidValue[] ValidatePropertyValue(object entity, string propertyName)
		{
			if (entity == null)
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;
			
			System.Type entityType = entity.GetType();

			if (!ClassValidator.ShouldNeedValidation(entityType))
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			ValidatableElement element = GetElementOrNew(entityType);

			return element.Validator.GetInvalidValues(entity, propertyName);
		}

		public InvalidValue[] ValidatePropertyValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression) where TEntity : class
		{
			if (entity == null)
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			System.Type entityType = entity.GetType();

			if (!ClassValidator.ShouldNeedValidation(entityType))
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;

			ValidatableElement element = GetElementOrNew(entityType);

			var propertyName = TypeUtils.DecodeMemberAccessExpression(expression).Name;
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
			IClassValidator cv = GetClassValidator(entityType);
			var element = new ValidatableElement(entityType, cv);
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
		/// Gets an acquaintance <see cref="IClassValidator"/>.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <returns>A acquaintance <see cref="IClassValidator"/> for the give type 
		/// or null if the the <typeparamref name="T"/> was never used in the engine instance.</returns>
		public IClassValidator GetValidator<T>()
		{
			return GetValidator(typeof(T));
		}

		private ValidatableElement GetElementOrNew(System.Type entityType)
		{
			ValidatableElement element;
			if (!validators.TryGetValue(entityType, out element))
			{
				IClassValidator cv = GetClassValidator(entityType);
				element = new ValidatableElement(entityType, cv);
				AddValidatableElement(element);
			}
			return element;
		}

		internal IClassValidator GetValidator(System.Type entityType)
		{
			ValidatableElement element;
			validators.TryGetValue(entityType, out element);

			return element == null ? null : element.Validator;
		}

		/// <summary>
		/// Gets a <see cref="IClassValidator"/> for a given <see cref="System.Type"/>
		/// </summary>
		/// <param name="entityType">The given <see cref="System.Type"/>.</param>
		/// <returns>
		/// A validator for a <see cref="System.Type"/> or null if the <paramref name="entityType"/>
		/// is not supported by <see cref="ClassValidator"/>.
		/// </returns>
		/// <remarks>
		/// In general a common application don't need to use this method but it can be useful for some kind of framework.
		/// </remarks>
		public IClassValidator GetClassValidator(System.Type entityType)
		{
			if (!ClassValidator.ShouldNeedValidation(entityType))
				return null;

			return factory.GetRootValidator(entityType);
		}

		private static T GetImplementation<T>(string classQualifiedName, string frendlyName) where T:class
		{
			if (!string.IsNullOrEmpty(classQualifiedName))
			{
				try
				{
					System.Type type = ReflectHelper.ClassForName(classQualifiedName);
					return (T)Activator.CreateInstance(type);
				}
				catch (MissingMethodException ex)
				{
					throw new ValidatorConfigurationException("Public constructor was not found at " + frendlyName + ": " + classQualifiedName, ex);
				}
				catch (InvalidCastException ex)
				{
					throw new ValidatorConfigurationException(
						"Type does not implement '" + typeof (T).FullName + "': " + classQualifiedName, ex);
				}
				catch (Exception ex)
				{
					throw new ValidatorConfigurationException("Unable to instanciate " + frendlyName + ": " + classQualifiedName, ex);
				}
			}
			return null;
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
