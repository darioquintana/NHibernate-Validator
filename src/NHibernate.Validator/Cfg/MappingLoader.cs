using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using NHibernate.Util;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Cfg
{
	public class MappingLoader
	{
		public const string MappingFileDefaultExtension = ".nhv.xml";

		private static readonly ILog log = LogManager.GetLogger(typeof(MappingLoader));

		private readonly Dictionary<System.Type, NhvClass> validatorMappings = new Dictionary<System.Type, NhvClass>();

		public void LoadMappings(IList<MappingConfiguration> mappings)
		{
			if (mappings == null)
				throw new ArgumentNullException("mappings");

			foreach (MappingConfiguration mc in mappings)
			{
				if (mc.IsEmpty())
					throw new ValidatorConfigurationException("<mapping> element in configuration specifies no attributes");
				if (!string.IsNullOrEmpty(mc.Resource))
				{
					log.Debug("Resource " + mc.Resource + " in " + mc.Assembly);
					AddResource(Assembly.Load(mc.Assembly), mc.Resource);
				}
				else if (!string.IsNullOrEmpty(mc.Assembly))
				{
					log.Debug("Assembly " + mc.Assembly);
					AddAssembly(mc.Assembly);
				}
				else if (!string.IsNullOrEmpty(mc.File))
				{
					log.Debug("File " + mc.File);
					AddFile(mc.File);
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

		public void AddResource(Assembly assembly, string resource)
		{
			log.Info("Mapping resource: " + resource);
			Stream stream = assembly.GetManifestResourceStream(resource);
			if (stream == null)
				throw new ValidatorConfigurationException("Resource " + resource + "not found in assembly " + assembly.FullName);

			try
			{
				AddInputStream(stream, resource);
			}
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not configure validator from resource " + resource, e);
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
			catch (ValidatorConfigurationException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not configure validator from input stream " + fileName, e);
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

			NhvValidator validator = parser.Parse(reader);
			foreach (NhvClass clazz in validator.@class)
			{
				AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(clazz.name, clazz.@namespace, clazz.assembly);
				System.Type type = ReflectHelper.TypeFromAssembly(fullClassName, true);
				log.Info("Full class name = " + type.AssemblyQualifiedName);
				validatorMappings[type] = clazz;
			}
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
			catch (MappingException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new ValidatorConfigurationException("Could not configure validator from input file " + filePath, e);
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
		}
	}
}