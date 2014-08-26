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
	public class FileExistsAttribute : EmbeddedRuleArgsAttribute
	{
		public FileExistsAttribute()
		{
			this.ErrorMessage = "{validator.fileExists}";
		}

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext constraintContext)
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