using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class FileExistsValidatorFixture
	{
		FileStream file = null;

		[Test]
		public void IsValid()
		{
			FileExistsValidator v = new FileExistsValidator();
			try
			{
				file = File.Create("testfile.tst");
				Assert.IsTrue(v.IsValid("testfile.tst"));
				Assert.IsTrue(v.IsValid(null));
				Assert.IsFalse(v.IsValid("nonexistingfile.fil"));
				Assert.IsFalse(v.IsValid(1));
			}
			finally
			{
				if (file != null)
				{
					file.Close();
					file.Dispose();
					File.Delete("testfile.tst");
				}
			}
		}
	}
}
