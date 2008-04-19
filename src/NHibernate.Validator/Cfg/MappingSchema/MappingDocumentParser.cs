using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public class MappingDocumentParser : IMappingDocumentParser
	{
		private readonly XmlSerializer serializer = new XmlSerializer(typeof(NhvMapping));

		public NhvMapping Parse(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			return (NhvMapping)serializer.Deserialize(stream);
		}
		public NhvMapping Parse(XmlReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return (NhvMapping)serializer.Deserialize(reader);
		}
	}
}