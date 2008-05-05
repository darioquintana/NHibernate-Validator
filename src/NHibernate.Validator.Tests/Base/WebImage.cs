using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Validator.Tests.Base
{
	public class WebImage
	{
		public int position;

		[FileExists]
		public string filename;
	}
}
