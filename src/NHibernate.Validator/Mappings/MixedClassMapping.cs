using System;
using System.Collections.Generic;
using System.Reflection;
using Iesi.Collections.Generic;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Mappings
{
	public abstract class MixedClassMapping : AbstractClassMapping
	{
		public MixedClassMapping(NhvmClass meta)
		{
			XmlClassMapping xmlcm = new XmlClassMapping(meta);
			ReflectionClassMapping rcm = new ReflectionClassMapping(xmlcm.EntityType);
			clazz = xmlcm.EntityType;

			InitializeClassAttributes(xmlcm, rcm);

			HashedSet<MemberInfo> lmembers = new HashedSet<MemberInfo>();
			InitializeMembers(lmembers, xmlcm, rcm);

			members = new List<MemberInfo>(lmembers).ToArray();
		}

		protected abstract void InitializeClassAttributes(XmlClassMapping xmlcm, ReflectionClassMapping rcm);
		protected abstract void InitializeMembers(HashedSet<MemberInfo> lmembers, XmlClassMapping xmlcm, ReflectionClassMapping rcm);

		protected void MixMembersWith(ISet<MemberInfo> lmembers, IClassMapping mapping)
		{
			foreach (MemberInfo info in mapping.GetMembers())
			{
				lmembers.Add(info);
				IEnumerable<Attribute> mas = mapping.GetMemberAttributes(info);
				if (mas != null)
				{
					List<Attribute> attrs;
					if (!membersAttributesDictionary.TryGetValue(info, out attrs))
					{
						membersAttributesDictionary[info] = new List<Attribute>(mas);
					}
					else
					{
						CombineAttribute(mas, attrs);
						membersAttributesDictionary[info] = attrs;
					}
				}
			}
		}

		protected static void CombineAttribute(IEnumerable<Attribute> origin, List<Attribute> dest)
		{
			foreach (Attribute ma in origin)
			{
				Attribute found = dest.Find(delegate(Attribute attribute)
				                            	{ return ma.TypeId.Equals(attribute.TypeId); });

				if (found != null && !AttributeUtils.AttributeAllowsMultiple(ma))
					dest.Remove(found);

				dest.Add(ma);
			}
		}
	}
}