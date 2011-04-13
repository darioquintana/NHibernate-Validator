using System;
using System.Collections.Generic;
using System.Reflection;


namespace NHibernate.Validator.Mappings
{
	public class ReflectionClassMapping : AbstractClassMapping
	{
		private const BindingFlags ValidatableMembers =
			BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			| BindingFlags.Static;

		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ReflectionClassMapping));

		public ReflectionClassMapping(System.Type clazz)
		{
			this.clazz = clazz;
		}

		private void CreateMemberAttributes(MemberInfo member)
		{
			object[] memberAttributes = member.GetCustomAttributes(false);

			foreach (Attribute memberAttribute in memberAttributes)
			{
				log.Debug(string.Format("For class {0} Adding member {1} to dictionary with attribute {2}", clazz.FullName,
				                        member.Name, memberAttribute));
				if (!membersAttributesDictionary.ContainsKey(member))
				{
					membersAttributesDictionary.Add(member, new List<Attribute>());
				}

				membersAttributesDictionary[member].Add(memberAttribute);
			}
		}

		#region Overrides of AbstractClassMapping

		protected override void Initialize()
		{
			// Create class attributes
			object[] attributes = clazz.GetCustomAttributes(false);
			classAttributes = new List<Attribute>(attributes.Length);

			foreach (Attribute attribute in clazz.GetCustomAttributes(false))
			{
				classAttributes.Add(attribute);
			}
			var lmember = new List<MemberInfo>();
			lmember.AddRange(clazz.GetFields(ValidatableMembers));
			lmember.AddRange(clazz.GetProperties(ValidatableMembers));
			members = lmember.ToArray();
			foreach (MemberInfo member in members)
			{
				CreateMemberAttributes(member);
			}
		}

		#endregion
	}
}