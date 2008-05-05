using System;
using System.IO;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class FileExistsValidator : IValidator
	{
		public bool IsValid(object value)
		{
			if (value == null)
				return true;

			if (!(value is string)) return false;

			string fileName = value.ToString();

			return File.Exists(fileName);
		}
	}
}
