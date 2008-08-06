using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// The file, where string is pointing to, must exist.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(FileExistsValidator))]
	public class FileExistsAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.fileExists}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
