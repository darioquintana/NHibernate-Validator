using System;
using System.Collections.Generic;
using System.Reflection;
using Iesi.Collections.Generic;
using NHibernate.Validator.Cfg.MappingSchema;

namespace NHibernate.Validator.Mappings
{
	public class XmlOverAttributeClassMapping : MixedClassMapping
	{

		public XmlOverAttributeClassMapping(NhvmClass meta) : base(meta) { }

		protected override void InitializeMembers(HashedSet<MemberInfo> lmembers, XmlClassMapping xmlcm, ReflectionClassMapping rcm)
		{
			MixMembersWith(lmembers, rcm);
			MixMembersWith(lmembers, xmlcm);
		}

		protected override void InitializeClassAttributes(XmlClassMapping xmlcm, ReflectionClassMapping rcm)
		{
			classAttributes = new List<Attribute>();
			CombineAttribute(rcm.GetClassAttributes(), classAttributes);
			CombineAttribute(xmlcm.GetClassAttributes(), classAttributes);
		}
	}
}
