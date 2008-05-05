using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;
using System.IO;

namespace NHibernate.Validator
{
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
