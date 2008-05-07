using System;
using System.Xml.XPath;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Configuration parsed values for a mapping XML node
	/// </summary>
	/// <remarks>
	/// There are 3 possible combinations of mapping attributes
	/// 1 - resource and assembly:  NHV will read the mapping resource from the specified assembly
	/// 2 - file only: NHV will read the mapping from the file.
	/// 3 - assembly only: NHV will find all the resources ending in nhv.xml from the assembly.
	/// </remarks>
	public class MappingConfiguration : IEquatable<MappingConfiguration>
	{
		private string assembly;
		private string file;
		private string resource;

		internal MappingConfiguration(XPathNavigator mappingElement)
		{
			Parse(mappingElement);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MappingConfiguration"/> class.
		/// </summary>
		/// <param name="file">Mapped file.</param>
		/// <exception cref="ArgumentException">When <paramref name="file"/> is null or empty.</exception>
		public MappingConfiguration(string file)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentException("file is null or empty.", "file");

			this.file = file;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MappingConfiguration"/> class.
		/// </summary>
		/// <param name="assembly">The assembly name.</param>
		/// <param name="resource">The mapped embedded resource.</param>
		/// <exception cref="ArgumentException">When <paramref name="assembly"/> is null or empty.</exception>
		public MappingConfiguration(string assembly, string resource)
		{
			if (string.IsNullOrEmpty(assembly))
				throw new ArgumentException("assembly is null or empty.", "assembly");

			this.assembly = assembly;
			this.resource = resource;
		}

		/// <summary>
		/// The file of a mapped validator.
		/// </summary>
		public string File
		{
			get { return file; }
		}

		/// <summary>
		/// The assembly name where validator mapping is.
		/// </summary>
		public string Assembly
		{
			get { return assembly; }
		}

		/// <summary>
		/// The name of an embedded resource in the <see cref="Assembly"/>.
		/// </summary>
		public string Resource
		{
			get { return resource; }
		}

		#region IEquatable<MappingConfiguration> Members

		public bool Equals(MappingConfiguration other)
		{
			if (other == null)
			{
				return false;
			}

			// file assigned and equals
			if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(other.file))
			{
				return file.Equals(other.file);
			}

			// this or other have only assembly assigned (include an assembly mean include all its resources)
			if ((!string.IsNullOrEmpty(assembly) && !string.IsNullOrEmpty(other.assembly))
			    && (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(other.resource)))
			{
				return assembly.Equals(other.assembly);
			}

			// this and other have both assembly&resource assigned
			if (!string.IsNullOrEmpty(assembly) && !string.IsNullOrEmpty(resource) && !string.IsNullOrEmpty(other.assembly)
			    && !string.IsNullOrEmpty(other.resource))
			{
				return assembly.Equals(other.assembly) && resource.Equals(other.resource);
			}

			// invalid or empty
			return false;
		}

		#endregion

		private bool IsValid()
		{
			// validate consistent (all empty ignore the element)
			return
				(!string.IsNullOrEmpty(assembly) && string.IsNullOrEmpty(file))
				|| (!string.IsNullOrEmpty(file) && string.IsNullOrEmpty(assembly))
				|| (!string.IsNullOrEmpty(resource) && !string.IsNullOrEmpty(assembly)) || IsEmpty();
		}

		private void Parse(XPathNavigator mappingElement)
		{
			if (mappingElement.MoveToFirstAttribute())
			{
				do
				{
					switch (mappingElement.Name)
					{
						case "assembly":
							assembly = mappingElement.Value;
							break;
						case "resource":
							resource = mappingElement.Value;
							break;
						case "file":
							file = mappingElement.Value;
							break;
					}
				}
				while (mappingElement.MoveToNextAttribute());
			}
			if (!IsValid())
			{
				throw new ValidatorConfigurationException(
					string.Format(
						@"Ambiguous mapping tag in configuration assembly={0} resource={1} file={2};
There are 3 possible combinations of mapping attributes
	1 - resource & assembly:  NHV will read the mapping resource from the specified assembly
	2 - file only: NHV will read the mapping from the file.
	3 - assembly only: NHV will find all the resources ending in hbm.xml from the assembly.",
						assembly, resource, file));
			}
		}

		/// <summary>
		/// Is the mapping element empty
		/// </summary>
		/// <returns>True if the element is empty; false otherwise.</returns>
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(resource) && string.IsNullOrEmpty(assembly) && string.IsNullOrEmpty(file);
		}

		public override string ToString()
		{
			return string.Format("file='{0}';assembly='{1}';resource='{2}'", file, assembly, resource);
		}
	}
}