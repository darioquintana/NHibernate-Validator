using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Mappings
{
	[TestFixture]
	public class XmlClassMappingFixture
	{
		private static NhvmClass GetNhvClassFor(System.Type currentClass)
		{
			NhvMapping mapp = XmlMappingLoader.GetXmlMappingFor(currentClass);
			if (mapp != null && mapp.@class.Length > 0)
				return mapp.@class[0];
			return null;
		}

		[Test]
		public void ClassAttributes()
		{
			XmlClassMapping rm = new XmlClassMapping(GetNhvClassFor(typeof(Address)));
			List<Attribute> mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(0, mi.Count);

			rm = new XmlClassMapping(GetNhvClassFor(typeof(Suricato)));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(1, mi.Count);
		}

		[Test]
		public void GetEntityType()
		{
			XmlClassMapping rm = new XmlClassMapping(GetNhvClassFor(typeof(Address)));
			Assert.AreEqual(typeof(Address), rm.EntityType);
		}

		[Test]
		public void MemberAttributes()
		{
			XmlClassMapping rm = new XmlClassMapping(GetNhvClassFor(typeof(Address)));
			MemberInfo mi = typeof(Address).GetField("floor");
			List<Attribute> mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof(Address).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof(Address).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);
		}

		[Test]
		public void Members()
		{
			XmlClassMapping rm = new XmlClassMapping(GetNhvClassFor(typeof(Address)));
			List<MemberInfo> mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(8, mi.Count);
		}

		[Test]
		public void InvalidPropertyName()
		{
			var map = new XmlClassMapping(GetNhvClassFor(typeof(A)));
			Assert.Throws<InvalidPropertyNameException>(() => map.GetMembers());
		}
	}
}
