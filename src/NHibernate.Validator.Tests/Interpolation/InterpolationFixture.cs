using System.Globalization;
using System.Reflection;
using System.Resources;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Interpolator;
using NHibernate.Validator.Tests.Base;
using SharpTestsEx;
using RangeAttribute = NHibernate.Validator.Constraints.RangeAttribute;
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
			Assert.AreEqual("{notpresent.Key} and {key.notPresent} and {key.notPresent2} 1", invalidValues[0].Message,
			                "Missing key should be left unchanged");
		}

		[Test]
		public void InterpolatingValues()
		{
			var result = GetInitializedInterpolator().Interpolate("The value of foo is ${Number} + {Number}", new Foo { Number = 82 }, new RangeValidator(), null);
			Assert.AreEqual("The value of foo is 82 + 12", result);
		}

		[Test]
		public void InterpolatingValues_WrongMember()
		{
			ActionAssert.Throws<InvalidPropertyNameException>(
				() =>
				GetInitializedInterpolator().Interpolate("The value of foo is ${WrongMember}.", new Foo {Number = 82},
				                                         new RangeValidator(), null));
		}

		public IMessageInterpolator GetInitializedInterpolator()
		{
			var rm = new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			var culture = new CultureInfo("en");

			var interpolator = new DefaultMessageInterpolator();
			interpolator.Initialize(rm, rm, culture);
			interpolator.Initialize(new RangeAttribute(2, 10));
			return interpolator;
		}
        
		public class Foo
		{
			public int Number { get; set; }
		}
	}
}
