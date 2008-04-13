using System.IO;
using System.Xml;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public interface IMappingDocumentParser
	{
		NhvValidator Parse(Stream stream);
		NhvValidator Parse(XmlReader reader);
	}
}