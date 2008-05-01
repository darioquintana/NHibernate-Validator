using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Mappings
{
	[TestFixture]
	public class ReflectionClassMappingFixture
	{
		[Test]
		public void ClassAttributes()
		{
			ReflectionClassMapping rm = new ReflectionClassMapping(typeof (Address));
			List<Attribute> mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(0, mi.Count);

			rm = new ReflectionClassMapping(typeof (Suricato));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(1, mi.Count);
		}

		[Test]
		public void GetEntityType()
		{
			ReflectionClassMapping rm = new ReflectionClassMapping(typeof (Address));
			Assert.AreEqual(typeof (Address), rm.EntityType);
		}

		[Test]
		public void MemberAttributes()
		{
			ReflectionClassMapping rm = new ReflectionClassMapping(typeof (Address));
			MemberInfo mi = typeof (Address).GetField("floor");
			List<Attribute> mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof (Address).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof (Address).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);
		}

		[Test]
		public void Members()
		{
			ReflectionClassMapping rm = new ReflectionClassMapping(typeof (Address));
			List<MemberInfo> mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(16, mi.Count);

			rm = new ReflectionClassMapping(typeof(Polimorphism.DerivatedClass));
			mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(1, mi.Count);
		}
	}
}