using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public class MappingDocumentParser : IMappingDocumentParser
	{
		private readonly XmlSerializer serializer = new XmlSerializer(typeof(NhvMapping));

		public NhvMapping Parse(XmlReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			NhvMapping result = (NhvMapping)serializer.Deserialize(reader);
			// The XmlSerializer not support the OnDeserializedAttribute so the only way
			// we have to manage a sort of Deserialization-Callback is here
			foreach (NhvmClass clas in result.@class)
			{
				clas.rootMapping = result;
			}
			return result;
		}
	}
}