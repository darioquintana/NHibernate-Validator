using System.Globalization;
using System.Resources;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ResourceFixture
{
	[TestFixture]
	public class ResourcesTest
	{
		[Test]
		public void CanGetResources()
		{
			ResourceManager rm = new ResourceManager(Environment.BaseNameOfMessageResource, typeof (IClassValidator).Assembly);
			Assert.IsNotNull(rm.GetString("validator.length", CultureInfo.InvariantCulture));

			string s_es = rm.GetString("validator.length", new CultureInfo("es"));
			string s_it = rm.GetString("validator.length", new CultureInfo("it"));

			Assert.AreNotEqual(s_es, s_it);
		}
	}
}