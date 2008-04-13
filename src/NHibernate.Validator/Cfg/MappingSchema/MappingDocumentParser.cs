using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NHibernate.Validator.Cfg.MappingSchema
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