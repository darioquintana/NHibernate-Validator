using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Util;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Mappings
{
	public class XmlClassMapping : AbstractClassMapping
	{
#if NETFX
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(XmlClassMapping));
#else
		private static readonly INHibernateLogger Log = NHibernateLogger.For(typeof(XmlClassMapping));
#endif
		private readonly NhvmClass meta;

		public XmlClassMapping(NhvmClass meta)
		{
			this.meta = meta;
		}

		#region Overrides of AbstractClassMapping

		protected override void Initialize()
		{
			clazz =
				ReflectHelper.ClassForFullName(
					TypeNameParser.Parse(meta.name, meta.rootMapping.@namespace, meta.rootMapping.assembly).ToString());
			classAttributes = meta.attributename == null
				? new List<Attribute>(0)
				: new List<Attribute>(meta.attributename.Length);

			List<MemberInfo> lmembers = meta.property == null
				? new List<MemberInfo>(0)
				: new List<MemberInfo>(meta.property.Length);

			if (meta.attributename != null)
			{
#if NETFX
				log.Debug("Looking for class attributes");
#else
				Log.Debug("Looking for class attributes");
#endif
				foreach (NhvmClassAttributename attributename in meta.attributename)
				{
#if NETFX
					log.Info("Attribute to look for = " + GetText(attributename));
#else
					Log.Info("Attribute to look for = {0}", GetText(attributename));
#endif
					Attribute classAttribute = RuleAttributeFactory.CreateAttributeFromClass(clazz, attributename);
					classAttributes.Add(classAttribute);
				}
			}

			if (meta.property != null)
			{
				foreach (NhvmProperty property in meta.property)
				{
					MemberInfo currentMember = TypeUtils.GetPropertyOrField(clazz, property.name);

					if (currentMember == null)
					{
						throw new InvalidPropertyNameException(property.name, clazz);
					}

#if NETFX
					log.Info("Looking for rules for property : " + property.name);
#else
					Log.Info("Looking for rules for property: {0}", property.name);
#endif
					lmembers.Add(currentMember);

					// creation of member attributes
					foreach (object rule in property.Items)
					{
						Attribute thisAttribute = RuleAttributeFactory.CreateAttributeFromRule(rule, meta.rootMapping.assembly,
						                                                                       meta.rootMapping.@namespace);

						if (thisAttribute != null)
						{
#if NETFX
							log.Info(string.Format("Adding member {0} to dictionary with attribute {1}", currentMember.Name, thisAttribute));
#else
							Log.Info("Adding member {0} to dictionary with attribute {1}", currentMember.Name, thisAttribute);
#endif
							if (!membersAttributesDictionary.ContainsKey(currentMember))
							{
								membersAttributesDictionary.Add(currentMember, new List<Attribute>());
							}

							membersAttributesDictionary[currentMember].Add(thisAttribute);
						}
					}
				}
			}
			members = lmembers.ToArray();
		}

		private static string GetText(NhvmClassAttributename attributename)
		{
			string[] text = attributename.Text;
			if (text != null)
			{
				string result = string.Join(Environment.NewLine, text).Trim();
				return result.Length == 0 ? null : result;
			}
			else
				return null;
		}

		#endregion
	}
}
