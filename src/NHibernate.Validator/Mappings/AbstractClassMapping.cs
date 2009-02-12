using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Validator.Mappings
{
	public abstract class AbstractClassMapping : IClassMapping
	{
		private bool initialized;
		protected System.Type clazz;
		protected List<Attribute> classAttributes;
		protected IEnumerable<MemberInfo> members;
		protected Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary = new Dictionary<MemberInfo, List<Attribute>>();

		#region IClassMapping Members

		public System.Type EntityType
		{
			get
			{
				DoInitialize();
				return clazz;
			}
		}

		private void DoInitialize()
		{
			if (!initialized)
			{
				Initialize();
				initialized = true;
			}
		}

		protected abstract void Initialize();

		public IEnumerable<Attribute> GetClassAttributes()
		{
			DoInitialize();
			return classAttributes.AsReadOnly();
		}

		public IEnumerable<MemberInfo> GetMembers()
		{
			DoInitialize();
			return members;
		}

		public IEnumerable<Attribute> GetMemberAttributes(MemberInfo member)
		{
			DoInitialize();
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