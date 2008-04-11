using System;
using System.Collections.Generic;
using System.Xml;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Exceptions;
using System.Resources;

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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
			return null;
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
			return false;
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given <see cref="System.Type"/>.
		/// </summary>
		/// <typeparam name="T">The entity type</typeparam>
		/// <param name="propertyName">The name of a property</param>
		/// <param name="value">The value of the property.</param>
		/// <returns>All the failures.</returns>
		/// <remarks>
		/// If the <typeparamref name="T"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] ValidatePropertyValue<T>(string propertyName, object value)
		{
			return ValidatePropertyValue(typeof (T), propertyName, value);
		}

		/// <summary>
		/// Use the <see cref="ClassValidator.GetPotentialInvalidValues(string, object)"/> for a given entity instance.
		/// </summary>
		/// <param name="entity">The entity instance to validate</param>
		/// <param name="propertyName">The name of a property</param>
		/// <returns>All the failures.</returns>
		/// <remarks>
		/// If the <see cref="System.Type"/> of the <paramref name="entity"/> was never inspected, or
		/// it was not configured, the <see cref="IClassValidator"/> will be automatic added to the engine.
		/// </remarks>
		public InvalidValue[] ValidatePropertyValue(object entity, string propertyName)
		{
			return null;
		}

		internal InvalidValue[] ValidatePropertyValue(System.Type entityType, string propertyName, object value)
		{
			IClassValidator cv = GetValidator(entityType);
			if(cv != null)
			{
				return cv.GetPotentialInvalidValues(propertyName, value);
			}
			else
				return ClassValidator.EMPTY_INVALID_VALUE_ARRAY;
		}

		/// <summary>
		/// Add a validator to the engine.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <remarks>
		/// Create an istance of <see cref="IClassValidator"/> for the given <typeparamref name="T"/>.
		/// </remarks>
		public void AddValidator<T>()
		{
			AddValidator(typeof (T), null);
		}

		internal void AddValidator(System.Type entityType, ResourceManager resource)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get a knowed <see cref="IClassValidator"/>.
		/// </summary>
		/// <typeparam name="T">The type of an entity.</typeparam>
		/// <returns>A konowed <see cref="IClassValidator"/> for the give type 
		/// or null if the the <typeparamref name="T"/> was never used in the engine instance.</returns>
		public IClassValidator GetValidator<T>()
		{
			return GetValidator(typeof (T));
		}

		internal IClassValidator GetValidator(System.Type entityType)
		{
			return null;
		}
	}
}
