using System.Globalization;
using System.Linq;
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
			var invalidValues = validator.GetInvalidValues(b);
			invalidValues.Should().Not.Be.Empty();
			invalidValues.Select(iv => iv.Message).Should("Missing key should be left unchanged").Contain(
				"{notpresent.Key} and {key.notPresent} and {key.notPresent2} 1");
		}

		[Test]
		public void InterpolatingValues()
		{
			var info = new InterpolationInfo(typeof (Foo), new Foo {Number = 82}, "Number", new RangeValidator(), null,
			                                 "The value of foo is ${Number} + {Number}");
			var result = GetInitializedInterpolator().Interpolate(info);
			Assert.AreEqual("The value of foo is 82 + 12", result);
		}

		[Test]
		public void InterpolatingValues_WrongMember()
		{
			var info = new InterpolationInfo(typeof(Foo), new Foo { Number = 82 }, "Number", new RangeValidator(), null,
																		 "The value of foo is ${WrongMember}.");
			ActionAssert.Throws<InvalidPropertyNameException>(
				() =>
				GetInitializedInterpolator().Interpolate(info));
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
