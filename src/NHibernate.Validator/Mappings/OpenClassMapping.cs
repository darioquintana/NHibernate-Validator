using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Validator.Mappings
{
	public class OpenClassMapping<T> : IClassMapping where T:class
	{
		protected List<Attribute> classAttributes= new List<Attribute>(5);
		protected Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary = new Dictionary<MemberInfo, List<Attribute>>();

		#region Implementation of IClassMapping

		public System.Type EntityType
		{
			get { return typeof(T); }
		}

		public IEnumerable<Attribute> GetClassAttributes()
		{
			return classAttributes;
		}

		public IEnumerable<MemberInfo> GetMembers()
		{
			return membersAttributesDictionary.Keys;
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

		public void AddBeanValidator(Attribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			// TODO : check attribute in order to validate that it is a valid Bean-Validator attribute
			classAttributes.Add(attribute);
		}

		public void AddConstraint(PropertyInfo property, Attribute attribute)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			AddConstraint((MemberInfo)property, attribute);
		}

		public void AddConstraint(FieldInfo field, Attribute attribute)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			AddConstraint((MemberInfo)field, attribute);
		}

		private void AddConstraint(MemberInfo member, Attribute attribute)
		{
			if (!membersAttributesDictionary.ContainsKey(member))
				membersAttributesDictionary.Add(member, new List<Attribute>());
			membersAttributesDictionary[member].Add(attribute);
		}
	}
}