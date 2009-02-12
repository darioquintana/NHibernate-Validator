using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Mappings;
using NUnit.Framework;
using RangeAttribute=NHibernate.Validator.Constraints.RangeAttribute;

namespace NHibernate.Validator.Tests.Mappings
{
	[TestFixture]
	public class MixedClassMappingFixture
	{
		private static NhvmClass GetNhvClassFor(System.Type currentClass)
		{
			NhvMapping mapp = XmlMappingLoader.GetXmlMappingFor(currentClass);
			if (mapp != null && mapp.@class.Length > 0)
			{
				return mapp.@class[0];
			}
			return null;
		}

		private static IClassMapping GetXmlClassMapping(System.Type currentClass)
		{
			NhvmClass xmlDef = GetNhvClassFor(currentClass);
			return new XmlClassMapping(xmlDef);
		}

		[Test]
		public void ClassAttributes()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			var mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(0, mi.Count);

			rm = new AttributeOverXmlClassMapping(GetXmlClassMapping(typeof (MixSuricato)));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(2, mi.Count);

			rm = new XmlOverAttributeClassMapping(GetXmlClassMapping(typeof (MixSuricato)));
			mi = new List<Attribute>(rm.GetClassAttributes());
			Assert.AreEqual(2, mi.Count);
		}

		[Test]
		public void GetEntityType()
		{
			IClassMapping rm = new XmlOverAttributeClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			Assert.AreEqual(typeof (MixAddress), rm.EntityType);
		}

		[Test]
		public void MemberAttributes()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			MemberInfo mi = typeof (MixAddress).GetField("floor");
			var mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof (MixAddress).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof (MixAddress).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(2, mas.Count);
			foreach (var ma in mas)
			{
				var ra = ma as RangeAttribute;
				if (ra != null)
				{
					Assert.AreEqual(2000, ra.Max);
				}
			}

			rm = new XmlOverAttributeClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			mi = typeof (MixAddress).GetField("floor");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(1, mas.Count);

			mi = typeof (MixAddress).GetProperty("Zip");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(3, mas.Count);

			mi = typeof (MixAddress).GetProperty("Id");
			mas = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual(2, mas.Count);
			foreach (var ma in mas)
			{
				var ra = ma as RangeAttribute;
				if (ra != null)
				{
					Assert.AreEqual(9999, ra.Max);
				}
			}
		}

		[Test]
		public void Members()
		{
			IClassMapping rm = new AttributeOverXmlClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			var mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(16, mi.Count); // the members of the class by reflection

			rm = new XmlOverAttributeClassMapping(GetXmlClassMapping(typeof (MixAddress)));
			mi = new List<MemberInfo>(rm.GetMembers());
			Assert.AreEqual(16, mi.Count);
		}
	}
}