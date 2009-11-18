using System;
using System.IO;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The file, where string is pointing to, must exist.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class FileExistsAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private string message = "{validator.fileExists}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null)
			{
				return true;
			}

			if (!(value is string))
			{
				return false;
			}

			string fileName = value.ToString();

			return File.Exists(fileName);
		}

		#endregion

	}
}