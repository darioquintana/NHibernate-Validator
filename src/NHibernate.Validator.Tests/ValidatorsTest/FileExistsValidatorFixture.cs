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
		[Test]
		public void IsValid()
		{
			FileExistsValidator v = new FileExistsValidator();

			Assert.IsTrue(v.IsValid(Path.GetTempFileName()));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsFalse(v.IsValid("nonexistingfile.fil"));
			Assert.IsFalse(v.IsValid(1));
		}
	}
}
