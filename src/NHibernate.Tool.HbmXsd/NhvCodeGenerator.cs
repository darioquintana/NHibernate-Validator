using System;
using System.CodeDom;
using System.IO;
using System.Xml.Schema;

namespace NHibernate.Tool.NhvXsd
{
	/// <summary>
	/// Responsible for generating the NHibernate.Validator.MappingSchema.Nhv* classes from nhibernate-validator.xsd.
	/// </summary>
	public class NhvCodeGenerator : XsdCodeGenerator
	{
		private const string GeneratedCodeNamespace = "NHibernate.Validator.Cfg.MappingSchema";
		private const string MappingSchemaResourceName = "NHibernate.Tool.NhvXsd.nhv-mapping.xsd";

		/// <summary>Generates C# classes.</summary>
		/// <param name="outputFileName">The file to which the generated code is written.</param>
		public void Execute(string outputFileName)
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream(MappingSchemaResourceName))
			{
				if (stream == null)
					throw new InvalidOperationException($"Resource {MappingSchemaResourceName} not found");
				var schema = XmlSchema.Read(stream, null);
				Execute(outputFileName, GeneratedCodeNamespace, schema);
			}
		}

		/// <summary>
		/// Customizes the generated code to better conform to the NHibernate's coding conventions.
		/// </summary>
		/// <param name="code">The customizable code DOM.</param>
		/// <param name="sourceSchema">The source XML Schema.</param>
		protected override void CustomizeGeneratedCode(CodeNamespace code, XmlSchema sourceSchema)
		{
			new ImproveNhvTypeNamesCommand(code).Execute();
			new ImproveEnumFieldsCommand(code).Execute();

			// TODO: Rename class fields?
		}
	}
}
