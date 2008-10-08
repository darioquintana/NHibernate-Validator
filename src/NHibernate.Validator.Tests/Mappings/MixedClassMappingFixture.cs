using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Mappings;
using NUnit.Framework;
using RangeAttribute = NHibernate.Validator.Constraints.RangeAttribute;

namespace NHibernate.Validator.Tests.Mappings
{
	[TestFixture]
	public class MixedClassMappingFixture
	{
		private static NhvmClass GetNhvClassFor(System.Type currentClass)
		{
			NhvMapping mapp = MappingLoader.GetMappingFor(currentClass);
			if (mapp != null && mapp.@class.Length > 0)
				return mapp.@class[0];
			return null;
		}

		[Test]
		public void ClassAttributes()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetNhvClassFor(typeof(MixAddress)));
			List<Attribute> mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(0, mi.Count);

			rm = new AttributeOverXmlClassMapping(GetNhvClassFor(typeof(MixSuricato)));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(2, mi.Count);

			rm = new XmlOverAttributeClassMapping(GetNhvClassFor(typeof(MixSuricato)));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(2, mi.Count);
		}

		[Test]
		public void GetEntityType()
		{
			IClassMapping rm = new XmlOverAttributeClassMapping(GetNhvClassFor(typeof(MixAddress)));
			Assert.AreEqual(typeof(MixAddress), rm.EntityType);
		}

		[Test]
		public void MemberAttributes()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetNhvClassFor(typeof(MixAddress)));
			MemberInfo mi = typeof(MixAddress).GetField("floor");
			List<Attribute> mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof(MixAddress).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof(MixAddress).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(2, mas.Count);
			foreach (Attribute ma in mas)
			{
				RangeAttribute ra = ma as RangeAttribute;
				if (ra != null)
					Assert.AreEqual(2000, ra.Max);
			}

			rm = new XmlOverAttributeClassMapping(GetNhvClassFor(typeof(MixAddress)));
			mi = typeof(MixAddress).GetField("floor");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof(MixAddress).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof(MixAddress).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(2, mas.Count);
			foreach (Attribute ma in mas)
			{
				RangeAttribute ra = ma as RangeAttribute;
				if (ra != null)
					Assert.AreEqual(9999, ra.Max);
			}
		}

		[Test]
		public void Members()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetNhvClassFor(typeof(MixAddress)));
			List<MemberInfo> mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(16, mi.Count); // the members of the class by reflection

			rm = new XmlOverAttributeClassMapping(GetNhvClassFor(typeof(MixAddress)));
			mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(16, mi.Count);
		}
	}
}
