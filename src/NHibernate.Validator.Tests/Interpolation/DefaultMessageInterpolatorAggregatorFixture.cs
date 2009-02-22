using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Interpolator;
using NUnit.Framework;
using RangeAttribute = NHibernate.Validator.Constraints.RangeAttribute;

namespace NHibernate.Validator.Tests.Interpolation
{
	[TestFixture]
	public class DefaultMessageInterpolatorAggregatorFixture
	{
		[Test, Ignore("Not supported yet.")]
		public void Serialization()
		{
			ResourceManager defrm =
				new ResourceManager(Cfg.Environment.BaseNameOfMessageResource,
														typeof(DefaultMessageInterpolatorAggregator).Assembly);
			ResourceManager custrm =
				new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			CultureInfo culture = new CultureInfo("en");

			DefaultMessageInterpolatorAggregator mia = new DefaultMessageInterpolatorAggregator();

			mia.Initialize(custrm, defrm, culture);
			RangeAttribute a = new RangeAttribute(2, 10);
			RangeValidator va = new RangeValidator();
			va.Initialize(a);
			mia.AddInterpolator(a, va);
			string originalMessage = mia.GetAttributeMessage(va);
			Assert.IsFalse(string.IsNullOrEmpty(originalMessage));
			using (MemoryStream memory = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(memory, mia);

				memory.Position = 0;
				DefaultMessageInterpolatorAggregator dmia = (DefaultMessageInterpolatorAggregator)formatter.Deserialize(memory);

				// follow instruction is what the owing a reference of interpolator must do
				dmia.Initialize(custrm, defrm, culture);

				Assert.AreEqual(originalMessage, dmia.GetAttributeMessage(va));
				/* TODO : To make the serialization of agregator really work we need a sort of
				 * "Validator ID" to make work the interpolators dictionary inside DefaultMessageInterpolatorAggregator.
				 * So far it work using the validator instance but after a deserialization the instance change 
				 * so is imposible to find it again (change the reference).
				 * Consideration to create "Validator ID" are:
				 * - Each Validator is linked to a bean member
				 * - Each bean member have an instance of an attribute initialized with values.
				 * - A bean can have the same validator more than one time but with different attribute instance
				 *	(different values of Message for example).
				 * 
				 * Note: if each Validator overrides Equals this test would pass, but it's too much invasive
				 */
			}
		}

		[Test]
		public void Interpolate()
		{
			ResourceManager defrm =
	new ResourceManager(Cfg.Environment.BaseNameOfMessageResource,
											typeof(DefaultMessageInterpolatorAggregator).Assembly);
			ResourceManager custrm =
				new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			CultureInfo culture = new CultureInfo("en");

			DefaultMessageInterpolatorAggregator mia = new DefaultMessageInterpolatorAggregator();
			DefaultMessageInterpolator dmi = new DefaultMessageInterpolator();
			mia.Initialize(custrm, defrm, culture);
			RangeValidator va = new RangeValidator();
			RangeAttribute a = new RangeAttribute(2, 10);

			Assert.AreEqual(a.Message, mia.Interpolate(a.Message, new object(), va, dmi));

			mia.AddInterpolator(a, va);
			Assert.AreNotEqual(a.Message, mia.Interpolate(a.Message, new object(), va, dmi));
		}

		[Test]
		public void GetAttributeMessage()
		{
			DefaultMessageInterpolatorAggregator mia = new DefaultMessageInterpolatorAggregator();
			RangeValidator va = new RangeValidator();
			try
			{
				mia.GetAttributeMessage(va);
			}
			catch (AssertionFailureException)
			{
				// Ok
			}
			ResourceManager defrm =
				new ResourceManager(Cfg.Environment.BaseNameOfMessageResource,
				                    typeof (DefaultMessageInterpolatorAggregator).Assembly);
			ResourceManager custrm =
				new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			CultureInfo culture = new CultureInfo("en");

			mia.Initialize(custrm, defrm, culture);
			RangeAttribute a = new RangeAttribute(2, 10);
			va.Initialize(a);
			mia.AddInterpolator(a, va);
			Assert.IsFalse(string.IsNullOrEmpty(mia.GetAttributeMessage(va)));
		}

		[Test,Ignore("Not supported yet")]
		public void InterpolatingValues()
		{
			var defrm = new ResourceManager(Cfg.Environment.BaseNameOfMessageResource,typeof(DefaultMessageInterpolatorAggregator).Assembly);
			var custrm = new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			var culture = new CultureInfo("en");

			var interpolator = new DefaultMessageInterpolator();
			interpolator.Initialize(defrm,defrm,culture);
			interpolator.Initialize(new RangeAttribute(2, 10));
			var result = interpolator.Interpolate("The value of foo is ${Number}", new Foo { Number = 82 }, new RangeValidator(), null);
			Assert.AreEqual("The value of foo is 82",result);
		}

		public class Foo
		{
			public int Number { get; set; }
		}
	}
}
