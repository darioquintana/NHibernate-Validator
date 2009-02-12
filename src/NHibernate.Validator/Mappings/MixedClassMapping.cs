using System;
using System.Collections.Generic;
using System.Reflection;
using Iesi.Collections.Generic;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Mappings
{
	public abstract class MixedClassMapping : AbstractClassMapping
	{
		protected virtual void InitializeMembers(HashedSet<MemberInfo> lmembers, IClassMapping baseMap, IClassMapping alternativeMap)
		{
			MixMembersWith(lmembers, baseMap);
			MixMembersWith(lmembers, alternativeMap);
		}

		protected virtual void InitializeClassAttributes(IClassMapping baseMap, IClassMapping alternativeMap)
		{
			classAttributes = new List<Attribute>();
			CombineAttribute(baseMap.GetClassAttributes(), classAttributes);
			CombineAttribute(alternativeMap.GetClassAttributes(), classAttributes);
		}

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
				Attribute found = dest.Find(attribute => ma.TypeId.Equals(attribute.TypeId));

				if (found != null && !AttributeUtils.AttributeAllowsMultiple(ma))
					dest.Remove(found);

				dest.Add(ma);
			}
		}
	}
}