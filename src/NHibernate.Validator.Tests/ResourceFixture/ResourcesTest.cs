using System.Globalization;
using System.Resources;
using NHibernate.Validator.Cfg;
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
			var rm = new ResourceManager(Environment.BaseNameOfMessageResource, typeof (IClassValidator).Assembly);
			Assert.IsNotNull(rm.GetString("validator.length", CultureInfo.InvariantCulture));

			string s_en = rm.GetString("validator.length", CultureInfo.InvariantCulture);
			string s_de = rm.GetString("validator.length", new CultureInfo("de"));
			string s_es = rm.GetString("validator.length", new CultureInfo("es"));
			string s_fr = rm.GetString("validator.length", new CultureInfo("fr"));
			string s_it = rm.GetString("validator.length", new CultureInfo("it"));
			string s_lv = rm.GetString("validator.length", new CultureInfo("lv"));
			string s_nl = rm.GetString("validator.length", new CultureInfo("nl"));

			Assert.AreNotEqual(s_en, s_de);
			Assert.AreNotEqual(s_en, s_es);
			Assert.AreNotEqual(s_en, s_fr);
			Assert.AreNotEqual(s_en, s_it);
			Assert.AreNotEqual(s_en, s_lv);
			Assert.AreNotEqual(s_en, s_nl);
		}
	}
}