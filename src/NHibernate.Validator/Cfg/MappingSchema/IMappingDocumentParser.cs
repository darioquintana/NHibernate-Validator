using System.Xml;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public interface IMappingDocumentParser
	{
		NhvMapping Parse(XmlReader reader);
	}
}