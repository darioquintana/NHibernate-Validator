using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace NHibernate.Validator.Mappings
{
	public class ReflectionClassMapping : AbstractClassMapping
	{
		private const BindingFlags ValidatableMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		private static readonly ILog log = LogManager.GetLogger(typeof(ReflectionClassMapping));

		public ReflectionClassMapping(System.Type clazz)
		{
			this.clazz = clazz;

			// Create class attributes
			object[] attributes = clazz.GetCustomAttributes(false);
			classAttributes = new List<Attribute>(attributes.Length);

			foreach (Attribute attribute in clazz.GetCustomAttributes(false))
				classAttributes.Add(attribute);
			List<MemberInfo> lmember = new List<MemberInfo>();
			lmember.AddRange(clazz.GetFields(ValidatableMembers));
			lmember.AddRange(clazz.GetProperties(ValidatableMembers));
			members = lmember.ToArray();
			foreach (MemberInfo member in members)
			{
				CreateMemberAttributes(member);
			}
		}

		private void CreateMemberAttributes(MemberInfo member)
		{
			object[] memberAttributes = member.GetCustomAttributes(false);

			foreach (Attribute memberAttribute in memberAttributes)
			{
				log.Info(string.Format("Adding member {0} to dictionary with attribute {1}", member.Name, memberAttribute));
				if (!membersAttributesDictionary.ContainsKey(member))
				{
					membersAttributesDictionary.Add(member, new List<Attribute>());
				}

				membersAttributesDictionary[member].Add(memberAttribute);
			}
		}
	}
}
