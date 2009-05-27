using System;
using System.IO;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class FileExistsValidator : IValidator
	{
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