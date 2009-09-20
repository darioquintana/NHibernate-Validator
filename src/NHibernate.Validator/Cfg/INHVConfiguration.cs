using System.Collections.Generic;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Contract for configuration sources.
	/// </summary>
	public interface INHVConfiguration
	{
		/// <summary>
		/// Assembly qualified name for shared engine provider.
		/// </summary>
		string SharedEngineProviderClass { get; }

		/// <summary>
		/// Configured properties.
		/// </summary>
		IDictionary<string, string> Properties { get; }

		/// <summary>
		/// Configured Mappings.
		/// </summary>
		IList<MappingConfiguration> Mappings { get; }

		/// <summary>
		/// Configured Entity Type Inspectors.
		/// </summary>
		/// <seealso cref="Engine.IEntityTypeInspector"/>
		IEnumerable<System.Type> EntityTypeInspectors { get; }
	}
}