using System.Text;
using System.IO;
using NHibernate.Validator.MappingSchema;
using NUnit.Framework;
using log4net;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class SerializationFixture : BaseValidatorFixture
	{
		private static ILog log = LogManager.GetLogger(typeof(SerializationFixture));
		StringBuilder validatorString;
		IMappingDocumentParser parser;

		[SetUp]
		public void Init()
		{
			parser = new MappingDocumentParser();
			validatorString = new StringBuilder();
			
			validatorString.AppendLine("<validator xmlns=\"urn:nhibernate-validator-0.1\">");
			validatorString.AppendLine("<class name=\"TvOwner\" namespace=\"Validator.Test\" assembly=\"Validator.Test\">");
			validatorString.AppendLine("<property name=\"tv\">");
			validatorString.AppendLine("<rules>");
			validatorString.AppendLine("<not-null />");
			validatorString.AppendLine("</rules>");
			validatorString.AppendLine("</property>");
			validatorString.AppendLine("</class>");
			validatorString.AppendLine("</validator>");

			log.Info(validatorString);
		}

		[Test]
		public void CanParseValidator()
		{
			MemoryStream stream = new MemoryStream(validatorString.Length);
			byte[] bytes = Encoding.ASCII.GetBytes(validatorString.ToString());
			stream.Write(bytes, 0, bytes.Length);
			stream.Seek(0, SeekOrigin.Begin);
			NhvValidator validator = parser.Parse(stream);
			Assert.IsNotNull(validator);
		}
	}
}
