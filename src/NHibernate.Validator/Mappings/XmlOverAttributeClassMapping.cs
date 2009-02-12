using System.Collections.Generic;
using System.Reflection;
using Iesi.Collections.Generic;

namespace NHibernate.Validator.Mappings
{
	public class XmlOverAttributeClassMapping : MixedClassMapping
	{
		private readonly IClassMapping xmlcm;
		private readonly IClassMapping rcm;

		public XmlOverAttributeClassMapping(IClassMapping externalDef)
		{
			xmlcm = externalDef;
			rcm = new ReflectionClassMapping(xmlcm.EntityType);
			clazz = xmlcm.EntityType;
		}

		#region Overrides of AbstractClassMapping

		protected override void Initialize()
		{

			InitializeClassAttributes(rcm, xmlcm);

			var lmembers = new HashedSet<MemberInfo>();
			InitializeMembers(lmembers, rcm, xmlcm);

			members = new List<MemberInfo>(lmembers).ToArray();
		}

		#endregion
	}
}
