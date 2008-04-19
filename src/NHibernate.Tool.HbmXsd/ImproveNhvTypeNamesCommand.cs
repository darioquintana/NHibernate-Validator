using System.CodeDom;

namespace NHibernate.Tool.NhvXsd
{
	/// <summary>
	/// Responsible for customizing type names in the generated code to match the desired output.
	/// </summary>
	public class ImproveNhvTypeNamesCommand : ImproveTypeNamesCommand
	{
		public ImproveNhvTypeNamesCommand(CodeNamespace code)
			: base(code)
		{
		}

		/// <summary>
		/// Overrides the <see cref="ImproveTypeNamesCommand.GetNewTypeName"/> to add the Nhv prefix and
		/// handle several specials cases.
		/// </summary>
		protected override string GetNewTypeName(string originalName, string rootElementName)
		{
			const string Prefix = "Nhvm";
			if ("nhvmapping".Equals(originalName))
				return "NhvMapping";
			return Prefix + base.GetNewTypeName(originalName, rootElementName);
		}
	}
}