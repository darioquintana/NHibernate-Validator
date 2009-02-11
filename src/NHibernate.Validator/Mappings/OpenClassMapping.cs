using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Mappings
{
	public class OpenClassMapping<T> : IClassMapping where T : class
	{
		protected List<Attribute> classAttributes = new List<Attribute>(5);

		protected Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary =
			new Dictionary<MemberInfo, List<Attribute>>();

		#region Implementation of IClassMapping

		public System.Type EntityType
		{
			get { return typeof (T); }
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
			{
				return result.AsReadOnly();
			}
			else
			{
				return new Attribute[0];
			}
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
			AddMemberConstraint(property, attribute);
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
			AddMemberConstraint(field, attribute);
		}

		public void AddMemberConstraint(MemberInfo member, Attribute attribute)
		{
			List<Attribute> constraints;

			if (!membersAttributesDictionary.TryGetValue(member, out constraints))
			{
				constraints = new List<Attribute>();
				membersAttributesDictionary.Add(member, constraints);
			}
			Attribute found = constraints.Find(x => x.TypeId.Equals(attribute.TypeId));
			if (found == null || AttributeUtils.AttributeAllowsMultiple(attribute))
			{
				membersAttributesDictionary[member].Add(attribute);
			}
		}
	}
}