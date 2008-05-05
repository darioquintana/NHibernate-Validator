using System.Text;
using System.Xml;
using NHibernate.Validator.Cfg.MappingSchema;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class XmlSerializationFixture : BaseValidatorFixture
	{
		[Test]
		public void CanParseValidator()
		{
			StringBuilder validatorString = new StringBuilder();

			validatorString.AppendLine("<nhv-mapping xmlns=\"urn:nhibernate-validator-1.0\">");
			validatorString.AppendLine("<class name=\"Validator.Test.TvOwner, Validator.Test\">");
			validatorString.AppendLine("<property name=\"tv\">");
			validatorString.AppendLine("<not-null />");
			validatorString.AppendLine("</property>");
			validatorString.AppendLine("</class>");
			validatorString.AppendLine("</nhv-mapping>");
			XmlTextReader xml = new XmlTextReader(validatorString.ToString(), XmlNodeType.Document, null);
			IMappingDocumentParser parser = new MappingDocumentParser();
			NhvMapping validator = parser.Parse(xml);
			Assert.IsNotNull(validator);
		}
	}
}
