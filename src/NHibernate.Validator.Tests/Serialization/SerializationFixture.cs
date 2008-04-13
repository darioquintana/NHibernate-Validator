using System.Text;
using System.Xml;
using NHibernate.Validator.Cfg.MappingSchema;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class SerializationFixture : BaseValidatorFixture
	{
		[Test]
		public void CanParseValidator()
		{
			StringBuilder validatorString = new StringBuilder();

			validatorString.AppendLine("<validator xmlns=\"urn:nhibernate-validator-1.0\">");
			validatorString.AppendLine("<class name=\"TvOwner\" namespace=\"Validator.Test\" assembly=\"Validator.Test\">");
			validatorString.AppendLine("<property name=\"tv\">");
			validatorString.AppendLine("<not-null />");
			validatorString.AppendLine("</property>");
			validatorString.AppendLine("</class>");
			validatorString.AppendLine("</validator>");
			XmlTextReader xml = new XmlTextReader(validatorString.ToString(), XmlNodeType.Document, null);
			IMappingDocumentParser parser = new MappingDocumentParser();
			NhvValidator validator = parser.Parse(xml);
			Assert.IsNotNull(validator);
		}
	}
}
