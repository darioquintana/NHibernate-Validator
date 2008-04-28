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
			if (classAttributes != null)
				return classAttributes.AsReadOnly();
			else
				return new Attribute[0];
		}

		public IEnumerable<MemberInfo> GetMembers()
		{
			return members;
		}

		public IEnumerable<Attribute> GetMemberAttributes(MemberInfo member)
		{
			List<Attribute> result;
			membersAttributesDictionary.TryGetValue(member, out result);
			if (result != null)
				return result.AsReadOnly();
			else
				return new Attribute[0];
		}

		#endregion
	}
}