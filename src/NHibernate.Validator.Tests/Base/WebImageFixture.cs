using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Validator.Engine;
using System.IO;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class WebImageFixture : BaseValidatorFixture
	{
		[Test]
		public void ImageValidationTest()
		{
			WebImage webImage = new WebImage();
			webImage.position = 1;
			webImage.filename = "testunknown";

			IClassValidator validator = GetClassValidator(typeof(WebImage));
			InvalidValue[] invalids = validator.GetInvalidValues(webImage);
			Assert.AreEqual(1, invalids.Length);

			webImage.filename = Path.GetTempFileName();
			invalids = validator.GetInvalidValues(webImage);
			Assert.AreEqual(0, invalids.Length);
		}
	}
}
