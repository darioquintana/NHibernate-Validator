using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
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
