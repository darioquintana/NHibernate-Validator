using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class WebImage
	{
		public int position;

		[FileExists]
		public string filename;
	}
}
