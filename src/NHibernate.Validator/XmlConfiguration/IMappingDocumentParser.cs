using System.IO;
using System.Xml;
using NHibernate.Validator.MappingSchema;

namespace NHibernate.Validator.XmlConfiguration
{
	public interface IMappingDocumentParser
	{
		NhvValidator Parse(Stream stream);
		NhvValidator Parse(XmlReader reader);
	}
}