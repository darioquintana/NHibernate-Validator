using NUnit.Framework;
using NHibernate.Validator.Engine;
using System.IO;
using SharpTestsEx;

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
			validator.GetInvalidValues(webImage).Should().Not.Be.Empty();

			webImage.filename = Path.GetTempFileName();
			validator.GetInvalidValues(webImage).Should().Be.Empty();
		}
	}
}
