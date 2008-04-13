using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NHibernate.Validator.MappingSchema;
using NHibernate.Validator.XmlConfiguration;

namespace NHibernate.Validator.XmlConfiguration
{
	public class MappingDocumentParser : IMappingDocumentParser
	{
		private readonly XmlSerializer serializer = new XmlSerializer(typeof(NhvValidator));

		public NhvValidator Parse(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			return (NhvValidator)serializer.Deserialize(stream);
		}
		public NhvValidator Parse(XmlReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return (NhvValidator)serializer.Deserialize(reader);
		}
	}
}