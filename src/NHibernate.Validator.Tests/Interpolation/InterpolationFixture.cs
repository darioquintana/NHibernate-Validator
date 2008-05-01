using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Interpolation
{
	[TestFixture]
	public class InterpolationFixture
	{
		[Test]
		public void MissingKey()
		{
			Building b = new Building();
			b.Address = "2323 Younge St";
			ClassValidator validator = new ClassValidator(typeof (Building));
				validator.GetInvalidValues(b); // message should be interpolated lazily in DefaultMessageInterpolator

			b = new Building();
			b.Address = string.Empty;
			InvalidValue[] invalidValues = validator.GetInvalidValues(b);
			Assert.Greater(invalidValues.Length, 0);
			Assert.AreEqual("{notpresent.Key} and #{key.notPresent} and {key.notPresent2} 1", invalidValues[0].Message,
			                "Missing key should be left unchanged");
		}
	}
}
