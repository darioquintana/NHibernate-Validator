using System.Collections.Generic;
using System.Reflection;
using Iesi.Collections.Generic;

namespace NHibernate.Validator.Mappings
{
	public class AttributeOverXmlClassMapping : MixedClassMapping
	{
		private readonly IClassMapping xmlcm;
		private readonly IClassMapping rcm;

		public AttributeOverXmlClassMapping(IClassMapping externalDef) 
		{
			xmlcm = externalDef;
			rcm = new ReflectionClassMapping(xmlcm.EntityType);
			clazz = xmlcm.EntityType;
		}

		#region Overrides of AbstractClassMapping

		protected override void Initialize()
		{
			InitializeClassAttributes(xmlcm, rcm);

			var lmembers = new HashedSet<MemberInfo>();
			InitializeMembers(lmembers, xmlcm, rcm);

			members = new List<MemberInfo>(lmembers).ToArray();
		}

		#endregion
	}
}
