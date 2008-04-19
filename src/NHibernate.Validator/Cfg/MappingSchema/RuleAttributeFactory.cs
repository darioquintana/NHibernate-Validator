using System;
using System.Reflection;
using log4net;
using NHibernate.Util;
using System.Collections.Generic;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public class RuleAttributeFactory
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(RuleAttributeFactory));

		private static readonly Dictionary<System.Type, ConvertSchemaRule> wellKnownRules =
			new Dictionary<System.Type, ConvertSchemaRule>();

		static RuleAttributeFactory()
		{
			wellKnownRules[typeof(NhvmNotNull)] = ConvertToNotNull;
			wellKnownRules[typeof(NhvmNotEmpty)] = ConvertToNotEmpty;
			wellKnownRules[typeof(NhvmNotnullorempty)] = ConvertToNotNullOrEmpty;
			wellKnownRules[typeof(NhvmLength)] = ConvertToLength;
			wellKnownRules[typeof(NhvmSize)] = ConvertToSize;
			wellKnownRules[typeof(NhvmFuture)] = ConvertToFuture;
			wellKnownRules[typeof(NhvmPast)] = ConvertToPast;
			wellKnownRules[typeof(NhvmValid)] = ConvertToValid;
			wellKnownRules[typeof(NhvmEmail)] = ConvertToEmail;
			wellKnownRules[typeof(NhvmRange)] = ConvertToRange;
			wellKnownRules[typeof(NhvmMin)] = ConvertToMin;
			wellKnownRules[typeof(NhvmMax)] = ConvertToMax;
			wellKnownRules[typeof(NhvmAsserttrue)] = ConvertToAssertTrue;
			wellKnownRules[typeof(NhvmPattern)] = ConvertToPattern;
			wellKnownRules[typeof(NhvmRule)] = ConvertToRule;
			wellKnownRules[typeof(NhvmIpAddress)] = ConvertToIPAddress;
			wellKnownRules[typeof(NhvmDigits)] = ConvertToDigits;
		}

		internal static Attribute CreateAttributeFromRule(object rule)
		{
			ConvertSchemaRule converter;
			if (wellKnownRules.TryGetValue(rule.GetType(), out converter))
				return converter(rule);
			else
				return null;
		}

		private static Attribute ConvertToDigits(object rule)
		{
			NhvmDigits digitsRule = (NhvmDigits)rule;

			int fractionalDigits = 0;

			if (digitsRule.fractionalDigitsSpecified)
				fractionalDigits = digitsRule.fractionalDigits;

			int intDigits = digitsRule.integerDigits;

			DigitsAttribute thisAttribute = new DigitsAttribute(digitsRule.integerDigits, digitsRule.fractionalDigits);
			log.Info(string.Format("Converting to Digits attribute with integer digits {0}, fractional digits {1}", intDigits, fractionalDigits));

			if (digitsRule.message != null)
			{
				thisAttribute.Message = digitsRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToRule(object rule)
		{
			NhvmRule ruleRule = (NhvmRule)rule;

			string attribute = ruleRule.attribute;
			//TODO: manage default namespace and assembly
			AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(attribute, "", "");

			System.Type type = ReflectHelper.ClassForFullName(fullClassName.Type);
			log.Info("The type found for ruleRule = " + type.FullName);
			Attribute thisattribute = (Attribute)Activator.CreateInstance(type);
			log.Info("Attribute found = " + thisattribute);
			foreach (NhvmParam parameter in ruleRule.param)
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
			NhvmRange rangeRule = (NhvmRange)rule;

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
			NhvmPattern patternRule = (NhvmPattern)rule;

			log.Info("Converting to Pattern attribute");
			PatternAttribute thisAttribute = new PatternAttribute();
			thisAttribute.Regex = patternRule.regex;
			if (patternRule.message != null)
			{
				thisAttribute.Message = patternRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToIPAddress(object rule)
		{
			NhvmIpAddress ipAddressRule = (NhvmIpAddress)rule;
			log.Info("Converting to IP Address attribute");
			IPAddressAttribute thisAttribute = new IPAddressAttribute();

			if (ipAddressRule.message != null)
			{
				thisAttribute.Message = ipAddressRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToAssertTrue(object rule)
		{
			NhvmAsserttrue assertTrueRule = (NhvmAsserttrue)rule;

			log.Info("Converting to AssertTrue attribute");
			AssertTrueAttribute thisAttribute = new AssertTrueAttribute();
			if (assertTrueRule.message != null)
			{
				thisAttribute.Message = assertTrueRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMin(object rule)
		{
			NhvmMin minRule = (NhvmMin)rule;
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
			NhvmMax maxRule = (NhvmMax)rule;
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
			NhvmEmail emailRule = (NhvmEmail)rule;
			log.Info("Converting to Email attribute");
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
			NhvmPast pastRule = (NhvmPast)rule;
			log.Info("Converting to Past attribute");
			PastAttribute thisAttribute = new PastAttribute();
			if (pastRule.message != null)
			{
				thisAttribute.Message = pastRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToFuture(object rule)
		{
			NhvmFuture futureRule = (NhvmFuture)rule;
			log.Info("Converting to future attribute");
			FutureAttribute thisAttribute = new FutureAttribute();
			if (futureRule.message != null)
			{
				thisAttribute.Message = futureRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToSize(object rule)
		{
			NhvmSize sizeRule = (NhvmSize)rule;
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
			NhvmLength lengthRule = (NhvmLength)rule;
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
			NhvmNotEmpty notEmptyRule = (NhvmNotEmpty)rule;
			NotEmptyAttribute thisAttribute = new NotEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notEmptyRule.message != null)
			{
				thisAttribute.Message = notEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNullOrEmpty(object rule)
		{
			NhvmNotnullorempty notNullOrEmptyRule = (NhvmNotnullorempty)rule;
			NotNullOrEmptyAttribute thisAttribute = new NotNullOrEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notNullOrEmptyRule.message != null)
			{
				thisAttribute.Message = notNullOrEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNull(object rule)
		{
			NhvmNotNull notNullRule = (NhvmNotNull)rule;
			NotNullAttribute thisAttribute = new NotNullAttribute();
			log.Info("Converting to NotNullAttribute");

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

		private delegate Attribute ConvertSchemaRule(object schemaRule);
	}
}