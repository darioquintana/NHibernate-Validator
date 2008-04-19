using System.IO;
using System.Xml;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public interface IMappingDocumentParser
	{
		NhvMapping Parse(Stream stream);
		NhvMapping Parse(XmlReader reader);
	}
}