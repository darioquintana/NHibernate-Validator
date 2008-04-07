using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.MappingSchema;
using log4net;
using System.Reflection;
using NHibernate.Util;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.XmlConfiguration
{
	public class XmlRulesFactory
	{
		private static ILog log = LogManager.GetLogger(typeof(XmlRulesFactory));

		internal static Attribute CreateAttributeFromRule(object rule)
		{
			if (rule is NhvNotNull)
			{
				return ConvertToNotNull(rule);
			}

			if (rule is NhvNotEmpty)
			{
				return ConvertToNotEmpty(rule);
			}


			if (rule is NhvNotnullorempty)
			{
				return ConvertToNotNullOrEmpty(rule);
			}

			if (rule is NhvLength)
			{
				return ConvertToLength(rule);
			}

			if (rule is NhvSize)
			{
				return ConvertToSize(rule);
			}

			if (rule is NhvFuture)
			{
				return ConvertToFuture(rule);
			}

			if (rule is NhvPast)
			{
				return ConvertToPast(rule);
			}

			if (rule is NhvValid)
			{
				return ConvertToValid(rule);
			}

			if (rule is NhvEmail)
			{
				return ConvertToEmail(rule);
			}

			if (rule is NhvRange)
			{
				return ConvertToRange(rule);
			}

			if (rule is NhvMin)
			{
				return ConvertToMin(rule);
			}

			if (rule is NhvMax)
			{
				return ConvertToMax(rule);
			}

			if (rule is NhvAsserttrue)
			{
				return ConvertToAssertTrue(rule);
			}

			if (rule is NhvPattern)
			{
				return ConvertToPattern(rule);
			}

			if (rule is NhvRule)
			{
				return ConvertToRule(rule);
			}

			return null;
		}

		private static Attribute ConvertToRule(object rule)
		{
			NhvRule ruleRule = rule as NhvRule;
			string attribute = ruleRule.attribute;
			AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(attribute, ruleRule.@namespace, ruleRule.assembly);

			System.Type type = ReflectHelper.ClassForFullName(fullClassName.Type);
			log.Info("The type found for ruleRule = " + type.FullName);
			Attribute thisattribute = (Attribute)Activator.CreateInstance(type);
			log.Info("Attribute found = " + thisattribute.ToString());
			foreach (NhvParam parameter in ruleRule.param)
			{
				PropertyInfo propInfo = type.GetProperty(parameter.name);
				if (propInfo != null)
				{
					log.Info("propInfo value = " + parameter.value);
					propInfo.SetValue(thisattribute, parameter.value, null);
				}
				else
				{
					log.Info("Could not get the property for name = " + parameter.name);
				}
			}

			return thisattribute;
		}

		private static Attribute ConvertToRange(object rule)
		{
			NhvRange rangeRule = rule as NhvRange;
			long min = long.MinValue;
			long max = long.MaxValue;

			if (rangeRule.minSpecified)
				min = rangeRule.min;

			if (rangeRule.maxSpecified)
				max = rangeRule.max;

			log.Info(string.Format("Converting to Range attribute with min {0}, max {1}", min, max));
			RangeAttribute thisAttribute = new RangeAttribute();
			thisAttribute.Min = min;
			thisAttribute.Max = max;
			if (rangeRule.message != null)
			{
				thisAttribute.Message = rangeRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToPattern(object rule)
		{
			log.Info("Converting to Pattern attribute");
			NhvPattern patternRule = rule as NhvPattern;
			PatternAttribute thisAttribute = new PatternAttribute();
			thisAttribute.Regex = patternRule.regex;
			if (patternRule.message != null)
			{
				thisAttribute.Message = patternRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToAssertTrue(object rule)
		{
			log.Info("Converting to AssertTrue attribute");
			NhvAsserttrue assertTrueRule = rule as NhvAsserttrue;
			AssertTrueAttribute thisAttribute = new AssertTrueAttribute();
			if (assertTrueRule.message != null)
			{
				thisAttribute.Message = assertTrueRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMin(object rule)
		{
			NhvMin minRule = rule as NhvMin;
			long value = 0;

			if (minRule.valueSpecified)
				value = minRule.value;

			log.Info(string.Format("Converting to Min attribute with value {0}", value));
			MinAttribute thisAttribute = new MinAttribute();
			thisAttribute.Value = value;

			if (minRule.message != null)
			{
				thisAttribute.Message = minRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMax(object rule)
		{
			NhvMax maxRule = rule as NhvMax;
			long value = long.MaxValue;

			if (maxRule.valueSpecified)
				value = maxRule.value;

			log.Info(string.Format("Converting to Max attribute with value {0}", value));
			MaxAttribute thisAttribute = new MaxAttribute();
			thisAttribute.Value = value;

			if (maxRule.message != null)
			{
				thisAttribute.Message = maxRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToEmail(object rule)
		{
			log.Info("Converting to Email attribute");
			NhvEmail emailRule = rule as NhvEmail;
			EmailAttribute thisAttribute = new EmailAttribute();
			if (emailRule.message != null)
			{
				thisAttribute.Message = emailRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToValid(object rule)
		{
			log.Info("Converting to valid attribute");
			return new ValidAttribute();
		}

		private static Attribute ConvertToPast(object rule)
		{
			log.Info("Converting to Past attribute");
			NhvPast pastRule = rule as NhvPast;
			PastAttribute thisAttribute = new PastAttribute();
			if (pastRule.message != null)
			{
				thisAttribute.Message = pastRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToFuture(object rule)
		{
			log.Info("Converting to future attribute");
			NhvFuture futureRule = rule as NhvFuture;
			FutureAttribute thisAttribute = new FutureAttribute();
			if (futureRule.message != null)
			{
				thisAttribute.Message = futureRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToSize(object rule)
		{
			NhvSize sizeRule = rule as NhvSize;
			int min = int.MinValue;
			int max = int.MaxValue;

			if (sizeRule.minSpecified)
				min = sizeRule.min;

			if (sizeRule.maxSpecified)
				max = sizeRule.max;

			log.Info(string.Format("Converting to Size attribute with min {0}, max {1}", min, max));
			SizeAttribute thisAttribute = new SizeAttribute();
			thisAttribute.Min = min;
			thisAttribute.Max = max;
			if (sizeRule.message != null)
			{
				thisAttribute.Message = sizeRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToLength(object rule)
		{
			NhvLength lengthRule = rule as NhvLength;
			int min = 0;
			int max = int.MaxValue;

			if (lengthRule.minSpecified)
				min = lengthRule.min;

			if (lengthRule.maxSpecified)
				max = lengthRule.max;
			LengthAttribute thisAttribute = new LengthAttribute(lengthRule.min, lengthRule.max);
			log.Info(string.Format("Converting to Length attribute with min {0}, max {1}", min, max));

			if (lengthRule.message != null)
			{
				thisAttribute.Message = lengthRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotEmpty(object rule)
		{
			NotEmptyAttribute thisAttribute = new NotEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");
			NhvNotEmpty notEmptyRule = rule as NhvNotEmpty;
			if (notEmptyRule.message != null)
			{
				thisAttribute.Message = notEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNullOrEmpty(object rule)
		{
			NotNullOrEmptyAttribute thisAttribute = new NotNullOrEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");
			NhvNotnullorempty notNullOrEmptyRule = rule as NhvNotnullorempty;
			if (notNullOrEmptyRule.message != null)
			{
				thisAttribute.Message = notNullOrEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNull(object rule)
		{
			NotNullAttribute thisAttribute = new NotNullAttribute();
			log.Info("Converting to NotNullAttribute");
			NhvNotNull notNullRule = rule as NhvNotNull;
			if (notNullRule.message != null)
			{
				thisAttribute.Message = notNullRule.message;
			}

			return thisAttribute;
		}

		internal static Attribute CreateAttributeFromClass(System.Type currentClass, string attributename)
		{
			Assembly assembly = currentClass.Assembly;
			System.Type type = assembly.GetType(currentClass.Namespace + "." + attributename + "Attribute");

			if (type == null)
				return null;

			return (Attribute)Activator.CreateInstance(type);
		}
	}
}
