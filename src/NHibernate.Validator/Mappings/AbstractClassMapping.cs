using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Validator.Mappings
{
	public class AbstractClassMapping : IClassMapping
	{
		protected System.Type clazz;
		protected List<Attribute> classAttributes;
		protected MemberInfo[] members;
		protected Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary = new Dictionary<MemberInfo, List<Attribute>>();

		#region IClassMapping Members

		public System.Type EntityType
		{
			get { return clazz; }
		}

		public IEnumerable<Attribute> GetClassAttributes()
		{
			return classAttributes.AsReadOnly();
		}

		public IEnumerable<MemberInfo> GetMembers()
		{
			return members;
		}

		public IEnumerable<Attribute> GetMemberAttributes(MemberInfo member)
		{
			List<Attribute> result;
			membersAttributesDictionary.TryGetValue(member, out result);
			return result == null ? null : result.AsReadOnly();
		}

		#endregion
	}
}