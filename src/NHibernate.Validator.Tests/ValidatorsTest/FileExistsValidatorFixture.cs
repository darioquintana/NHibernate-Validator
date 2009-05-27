using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Constraints;
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

			Assert.IsTrue(v.IsValid(Path.GetTempFileName(), null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsFalse(v.IsValid("nonexistingfile.fil", null));
			Assert.IsFalse(v.IsValid(1, null));
		}
	}
}
