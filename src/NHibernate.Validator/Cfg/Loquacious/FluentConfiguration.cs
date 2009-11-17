using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg.Loquacious.Impl;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg.Loquacious
{
	public class FluentConfiguration : INHVConfiguration, IFluentConfiguration, IMappingLoader
	{
		private readonly FluentMappingLoader loader = new FluentMappingLoader();
		private readonly IList<MappingConfiguration> mappings = new List<MappingConfiguration>();
		private readonly IDictionary<string, string> properties = new Dictionary<string, string>();
		private readonly HashSet<System.Type> entityTypeInspectors = new HashSet<System.Type>();

		public FluentConfiguration()
		{
			SetExternalMappingsLoader<FluentMappingLoader>();
		}

		#region IFluentConfiguration Members

		public INhIntegration IntegrateWithNHibernate
		{
			get { return new NhIntegration(this); }
		}

		public IFluentConfiguration SetMessageInterpolator<T>() where T : IMessageInterpolator
		{
			properties[Environment.MessageInterpolatorClass] = typeof (T).AssemblyQualifiedName;
			return this;
		}

		public IFluentConfiguration SetConstraintValidatorFactory<T>() where T : IConstraintValidatorFactory
		{
			properties[Environment.ConstraintValidatorFactoryClass] = typeof(T).AssemblyQualifiedName;
			return this;
		}

		public IFluentConfiguration SetCustomResourceManager(string resourceBaseName, Assembly assembly)
		{
			properties[Environment.CustomResourceManager] = resourceBaseName + ", " + assembly.FullName;
			return this;
		}

		public IFluentConfiguration AddEntityTypeInspector<T>() where T : IEntityTypeInspector
		{
			entityTypeInspectors.Add(typeof (T));
			return this;
		}

		public IFluentConfiguration SetDefaultValidatorMode(ValidatorMode mode)
		{
			properties[Environment.ValidatorMode] = ValidatorModeConvertFrom(mode);
			return this;
		}

		public IFluentConfiguration Register<TDef, TEntity>()
			where TDef : IValidationDefinition<TEntity>, IMappingSource, new() where TEntity : class
		{
			loader.AddClassDefinition<TDef, TEntity>();
			return this;
		}

		public IFluentConfiguration Register<TEntity>(IValidationDefinition<TEntity> validationDefinition) where TEntity : class
		{
			loader.AddClassDefinition(validationDefinition);
			return this;
		}

		public IFluentConfiguration Register(IEnumerable<System.Type> definitions)
		{
			loader.AddClassDefinitions(definitions);
			return this;
		}

		#endregion

		#region IMappingLoader Members

		void IMappingLoader.LoadMappings(IList<MappingConfiguration> configurationMappings)
		{
			loader.LoadMappings(configurationMappings);
		}

		void IMappingLoader.AddAssembly(string assemblyName)
		{
			loader.AddAssembly(assemblyName);
		}

		void IMappingLoader.AddAssembly(Assembly assembly)
		{
			loader.AddAssembly(assembly);
		}

		IEnumerable<IClassMapping> IMappingsProvider.GetMappings()
		{
			return loader.GetMappings();
		}

		#endregion

		#region INHVConfiguration Members

		string INHVConfiguration.SharedEngineProviderClass
		{
			get { throw new NotSupportedException("SharedEngineProvider is ignored out of application configuration file."); }
		}

		IDictionary<string, string> INHVConfiguration.Properties
		{
			get { return properties; }
		}

		IList<MappingConfiguration> INHVConfiguration.Mappings
		{
			get { return mappings; }
		}

		IEnumerable<System.Type> INHVConfiguration.EntityTypeInspectors
		{
			get { return entityTypeInspectors; }
		}

		#endregion

		private void SetExternalMappingsLoader<T>() where T : IMappingLoader
		{
			properties[Environment.MappingLoaderClass] = typeof (T).AssemblyQualifiedName;
		}

		private static string ValidatorModeConvertFrom(ValidatorMode validatorMode)
		{
			switch (validatorMode)
			{
				case ValidatorMode.UseAttribute:
					return "useattribute";
				case ValidatorMode.UseExternal:
					return "useexternal";
				case ValidatorMode.OverrideAttributeWithExternal:
					return "overrideattributewithexternal";
				case ValidatorMode.OverrideExternalWithAttribute:
					return "overrideexternalwithattribute";
				default:
					throw new ArgumentOutOfRangeException("validatorMode");
			}
		}
	}
}