using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Interpolator;
using NUnit.Framework;
using RangeAttribute=NHibernate.Validator.Constraints.RangeAttribute;

namespace NHibernate.Validator.Tests.Interpolation
{
	[TestFixture]
	public class DefaultMessageInterpolatorAggregatorFixture
	{
		[Test]
		public void GetAttributeMessage()
		{
			var mia = new DefaultMessageInterpolatorAggregator();
			var va = new RangeValidator();
			try
			{
				mia.GetAttributeMessage(va);
			}
			catch (AssertionFailureException)
			{
				// Ok
			}
			var defrm =
				new ResourceManager(Environment.BaseNameOfMessageResource,
				                    typeof (DefaultMessageInterpolatorAggregator).Assembly);
			var custrm =
				new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			var culture = new CultureInfo("en");

			mia.Initialize(custrm, defrm, culture);
			var a = new RangeAttribute(2, 10);
			va.Initialize(a);
			mia.AddInterpolator(a, va);
			Assert.IsFalse(string.IsNullOrEmpty(mia.GetAttributeMessage(va)));
		}

		[Test]
		public void Interpolate()
		{
			var defrm = new ResourceManager(Environment.BaseNameOfMessageResource,
				                    typeof (DefaultMessageInterpolatorAggregator).Assembly);
			var custrm = new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			var culture = new CultureInfo("en");

			var mia = new DefaultMessageInterpolatorAggregator();
			var dmi = new DefaultMessageInterpolator();
			mia.Initialize(custrm, defrm, culture);
			var va = new RangeValidator();
			var a = new RangeAttribute(2, 10);

			var info = new InterpolationInfo(typeof (object), new object(), null, va, dmi, a.Message);
			Assert.AreEqual(a.Message, mia.Interpolate(info));

			mia.AddInterpolator(a, va);
			var info1 = new InterpolationInfo(typeof(object), new object(), null, va, dmi, a.Message);
			Assert.AreNotEqual(a.Message, mia.Interpolate(info1));
		}

		[Test, Ignore("Not supported yet.")]
		public void Serialization()
		{
			var defrm = new ResourceManager(Environment.BaseNameOfMessageResource,
			                                typeof (DefaultMessageInterpolatorAggregator).Assembly);
			var custrm = new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly());
			var culture = new CultureInfo("en");

			var mia = new DefaultMessageInterpolatorAggregator();

			mia.Initialize(custrm, defrm, culture);
			var a = new RangeAttribute(2, 10);
			var va = new RangeValidator();
			va.Initialize(a);
			mia.AddInterpolator(a, va);
			string originalMessage = mia.GetAttributeMessage(va);
			Assert.IsFalse(string.IsNullOrEmpty(originalMessage));
			using (var memory = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(memory, mia);

				memory.Position = 0;
				var dmia = (DefaultMessageInterpolatorAggregator) formatter.Deserialize(memory);

				// follow instruction is what the owing a reference of interpolator must do
				dmia.Initialize(custrm, defrm, culture);

				Assert.AreEqual(originalMessage, dmia.GetAttributeMessage(va));
				/* TODO : To make the serialization of agregator really work we need a sort of
				 * "Validator ID" to make work the interpolators dictionary inside DefaultMessageInterpolatorAggregator.
				 * So far it work using the validator instance but after a deserialization the instance change 
				 * so is imposible to find it again (change the reference).
				 * Consideration to create "Validator ID" are:
				 * - Each Validator is linked to a entity member
				 * - Each entity member have an instance of an attribute initialized with values.
				 * - A entity can have the same validator more than one time but with different attribute instance
				 *	(different values of Message for example).
				 * 
				 * Note: if each Validator overrides Equals this test would pass, but it's too much invasive
				 */
			}
		}
	}
}