using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Validator.Mappings
{
	public interface IClassMapping
	{
		System.Type EntityType { get;}
		IEnumerable<Attribute> GetClassAttributes();
		IEnumerable<MemberInfo> GetMembers();
		IEnumerable<Attribute> GetMemberAttributes(MemberInfo member);
	}
}