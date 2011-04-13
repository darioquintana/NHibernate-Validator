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
					log.Debug("Assembly " + mc.Assembly);
					AddAssembly(mc.Assembly);
				}
				else if (!string.IsNullOrEmpty(mc.Assembly) && !string.IsNullOrEmpty(mc.Resource))
				{
					log.Debug("Resource " + mc.Resource + " in " + mc.Assembly);
					AddResource(Assembly.Load(mc.Assembly), mc.Resource);
				}
				else if (!string.IsNullOrEmpty(mc.File))
				{
					log.Debug("File " + mc.File);
					AddFile(mc.File);
				}
				else
				{
					log.Warn(string.Format("Mapping configuration ignored: Assembly>{0}< Resource>{1}< File>{2}<", mc.Assembly,
					                       mc.Resource, mc.File));
				}
			}
		}

		public void AddAssembly(string assemblyName)
		{
			log.Info("Searching for mapped documents in assembly: " + assemblyName);

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
			log.Info("Mapping resource: " + resource);
			Stream stream = assembly.GetManifestResourceStream(resource);
			if (stream == null)
				throw new ValidatorConfigurationException("Resource " + resource + " not found in assembly " + assembly.FullName);

			try
			{
				AddInputStream(stream, resource);
			}
			finally
			{
				stream.Close();
			}
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
			catch(Exception e)
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
			log.Info("Mapping file: " + filePath);
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
			try
			{
				ml.AddResource(type.Assembly, resourceName);
			}
			catch (ValidatorConfigurationException)
			{
				return null;
			}
			return ml.Mappings[0];
		}

		public NhvMapping[] Mappings
		{
			get { return mappings.ToArray(); }
		}
	}
}