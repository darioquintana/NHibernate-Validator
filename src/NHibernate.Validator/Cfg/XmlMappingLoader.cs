using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;

namespace NHibernate.Validator.Cfg
{
	public class XmlMappingLoader : IMappingLoader
	{
		public const string MappingFileDefaultExtension = ".nhv.xml";

		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(XmlMappingLoader));
		private readonly List<NhvMapping> mappings= new List<NhvMapping>();

		public void LoadMappings(IList<MappingConfiguration> configurationMappings)
		{
			if (configurationMappings == null)
				throw new ArgumentNullException("configurationMappings");

			foreach (MappingConfiguration mc in configurationMappings)
			{
				if (!string.IsNullOrEmpty(mc.Assembly) && string.IsNullOrEmpty(mc.Resource))
				{
					log.DebugFormat("Assembly {0}", mc.Assembly);
					AddAssembly(mc.Assembly);
				}
				else if (!string.IsNullOrEmpty(mc.Assembly) && !string.IsNullOrEmpty(mc.Resource))
				{
					log.DebugFormat("Resource {0} in {1}", mc.Resource, mc.Assembly);
					AddResource(Assembly.Load(mc.Assembly), mc.Resource);
				}
				else if (!string.IsNullOrEmpty(mc.File))
				{
					log.DebugFormat("File {0}", mc.File);
					AddFile(mc.File);
				}
				else
				{
					log.WarnFormat(
						"Mapping configuration ignored: Assembly>{0}< Resource>{1}< File>{2}<",
						mc.Assembly, mc.Resource, mc.File);
				}
			}
		}

		public void AddAssembly(string assemblyName)
		{
			log.InfoFormat("Searching for mapped documents in assembly: {0}", assemblyName);

			Assembly assembly;
			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not add assembly " + assemblyName, e);
			}

			AddAssembly(assembly);
		}

		/// <summary>
		/// Adds all of the assembly's embedded resources whose names end with <c>.nhv.xml</c>.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>This configuration object.</returns>
		public void AddAssembly(Assembly assembly)
		{
			foreach (string resource in assembly.GetManifestResourceNames())
			{
				if (resource.EndsWith(MappingFileDefaultExtension))
					AddResource(assembly, resource);
			}
		}

		public IEnumerable<IClassMapping> GetMappings()
		{
			var classMappings = new List<IClassMapping>();
			foreach (var nhvMapping in mappings)
			{
				foreach (var nhvmClass in nhvMapping.@class)
				{
					classMappings.Add(new XmlClassMapping(nhvmClass));
				}
			}
			return classMappings;
		}

		public void AddResource(Assembly assembly, string resource)
		{
			AddResourceImpl(assembly, resource, true);
		}

		private bool AddResourceImpl(Assembly assembly, string resource, bool throwIfNoStream)
		{
			log.InfoFormat("Mapping resource: {0}", resource);
			Stream stream = assembly.GetManifestResourceStream(resource);
			if (stream == null)
			{
				if (throwIfNoStream)
					throw new ValidatorConfigurationException("Resource " + resource + " not found in assembly " + assembly.FullName);
				return false;
			}

			try
			{
				AddInputStream(stream, resource);
			}
			finally
			{
				stream.Close();
			}

			return true;
		}

		public void AddInputStream(Stream xmlInputStream, string fileName)
		{
			XmlTextReader textReader = null;
			try
			{
				textReader = new XmlTextReader(xmlInputStream);
				AddXmlReader(textReader, fileName);
			}
			finally
			{
				if (textReader != null)
					textReader.Close();
			}
		}

		public void AddXmlReader(XmlTextReader reader, string fileName)
		{
			IMappingDocumentParser parser = new MappingDocumentParser();
			try
			{
				AddMapping(parser.Parse(reader));
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not load file " + fileName, e);
			}
		}

		public void AddMapping(NhvMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			mappings.Add(mapping);
		}

		public void AddFile(string filePath)
		{
			log.InfoFormat("Mapping file: {0}", filePath);
			XmlTextReader textReader = null;
			try
			{
				textReader = new XmlTextReader(filePath);
				AddXmlReader(textReader, filePath);
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
		}

		/// <summary>
		/// Load and return a mapping for a given type.
		/// </summary>
		/// <param name="type">The given type.</param>
		/// <returns>The mapping.</returns>
		/// <remarks>
		/// The method use a convention to find the resource that represent the mapping for the given class.
		/// - The mapping must be compiled like embedded resource in the same assembly of the given type
		/// - The name o the resource must be the same name of the type and end with ".nhv.xml"
		/// - The resource must stay in the same namespace of the type
		/// </remarks>
		public static NhvMapping GetXmlMappingFor(System.Type type)
		{
			string resourceName = type.FullName + MappingFileDefaultExtension;
			var ml = new XmlMappingLoader();

			return !ml.AddResourceImpl(type.Assembly, resourceName, false) 
				? null 
				: ml.Mappings[0];
		}

		public NhvMapping[] Mappings
		{
			get { return mappings.ToArray(); }
		}
	}
}