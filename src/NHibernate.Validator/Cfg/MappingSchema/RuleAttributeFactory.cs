using System;
using System.Reflection;
using log4net;
using NHibernate.Util;
using System.Collections.Generic;
using NHibernate.Validator.Exceptions;

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

		public static Attribute CreateAttributeFromRule(object rule, string defaultAssembly, string defaultNameSpace)
		{
			// public Only for test scope
			ConvertSchemaRule converter;
			if (wellKnownRules.TryGetValue(rule.GetType(), out converter))
				return converter(new XmlNhvmRuleCoverterArgs(rule,defaultAssembly,defaultNameSpace));
			else
				return null;
		}

		/// <summary>
		/// Create the attribute of a bean validator from XML definitions.
		/// </summary>
		/// <param name="beanClass">The entity class where associate the attribute.</param>
		/// <param name="attributename">The attribute name in the mapping.</param>
		/// <returns>The <see cref="Attribute"/> instance.</returns>
		/// <remarks>
		/// We are using the conventions:
		/// - The attribute must be defined in the same namespace of the <paramref name="beanClass"/>.
		/// - The attribute class may have the postfix "Attribute" without need to use it in the mapping.
		/// </remarks>
		public static Attribute CreateAttributeFromClass(System.Type beanClass, string attributename)
		{
			// public Only for test scope
			Assembly assembly = beanClass.Assembly;
			System.Type type = assembly.GetType(beanClass.Namespace + "." + attributename + "Attribute");

			if (type == null)
			{
				type = assembly.GetType(beanClass.Namespace + "." + attributename);
			}

			if (type == null)
			{
				throw new InvalidAttributeNameException(attributename, beanClass);
			}

			return (Attribute)Activator.CreateInstance(type);
		}

		private static Attribute ConvertToDigits(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmDigits digitsRule = (NhvmDigits)rule.schemaRule;

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

		private static Attribute ConvertToRule(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmRule ruleRule = (NhvmRule)rule.schemaRule;

			string attribute = ruleRule.attribute;
			AssemblyQualifiedTypeName fullClassName =
				TypeNameParser.Parse(attribute, rule.defaultNameSpace, rule.defaultAssembly);

			System.Type type = ReflectHelper.ClassForFullName(fullClassName.ToString());
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

		private static Attribute ConvertToRange(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmRange rangeRule = (NhvmRange)rule.schemaRule;

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

		private static Attribute ConvertToPattern(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmPattern patternRule = (NhvmPattern)rule.schemaRule;

			log.Info("Converting to Pattern attribute");
			PatternAttribute thisAttribute = new PatternAttribute();
			thisAttribute.Regex = patternRule.regex;
			if (patternRule.message != null)
			{
				thisAttribute.Message = patternRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToIPAddress(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmIpAddress ipAddressRule = (NhvmIpAddress)rule.schemaRule;
			log.Info("Converting to IP Address attribute");
			IPAddressAttribute thisAttribute = new IPAddressAttribute();

			if (ipAddressRule.message != null)
			{
				thisAttribute.Message = ipAddressRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToAssertTrue(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmAsserttrue assertTrueRule = (NhvmAsserttrue)rule.schemaRule;

			log.Info("Converting to AssertTrue attribute");
			AssertTrueAttribute thisAttribute = new AssertTrueAttribute();
			if (assertTrueRule.message != null)
			{
				thisAttribute.Message = assertTrueRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMin(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmMin minRule = (NhvmMin)rule.schemaRule;
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

		private static Attribute ConvertToMax(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmMax maxRule = (NhvmMax)rule.schemaRule;
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

		private static Attribute ConvertToEmail(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmEmail emailRule = (NhvmEmail)rule.schemaRule;
			log.Info("Converting to Email attribute");
			EmailAttribute thisAttribute = new EmailAttribute();
			if (emailRule.message != null)
			{
				thisAttribute.Message = emailRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToValid(XmlNhvmRuleCoverterArgs rule)
		{
			log.Info("Converting to valid attribute");
			return new ValidAttribute();
		}

		private static Attribute ConvertToPast(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmPast pastRule = (NhvmPast)rule.schemaRule;
			log.Info("Converting to Past attribute");
			PastAttribute thisAttribute = new PastAttribute();
			if (pastRule.message != null)
			{
				thisAttribute.Message = pastRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToFuture(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmFuture futureRule = (NhvmFuture)rule.schemaRule;
			log.Info("Converting to future attribute");
			FutureAttribute thisAttribute = new FutureAttribute();
			if (futureRule.message != null)
			{
				thisAttribute.Message = futureRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToSize(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmSize sizeRule = (NhvmSize)rule.schemaRule;
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

		private static Attribute ConvertToLength(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmLength lengthRule = (NhvmLength)rule.schemaRule;
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

		private static Attribute ConvertToNotEmpty(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmNotEmpty notEmptyRule = (NhvmNotEmpty)rule.schemaRule;
			NotEmptyAttribute thisAttribute = new NotEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notEmptyRule.message != null)
			{
				thisAttribute.Message = notEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNullOrEmpty(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmNotnullorempty notNullOrEmptyRule = (NhvmNotnullorempty)rule.schemaRule;
			NotNullOrEmptyAttribute thisAttribute = new NotNullOrEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notNullOrEmptyRule.message != null)
			{
				thisAttribute.Message = notNullOrEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNull(XmlNhvmRuleCoverterArgs rule)
		{
			NhvmNotNull notNullRule = (NhvmNotNull)rule.schemaRule;
			NotNullAttribute thisAttribute = new NotNullAttribute();
			log.Info("Converting to NotNullAttribute");

			if (notNullRule.message != null)
			{
				thisAttribute.Message = notNullRule.message;
			}

			return thisAttribute;
		}

		private delegate Attribute ConvertSchemaRule(XmlNhvmRuleCoverterArgs schemaRule);
		private class XmlNhvmRuleCoverterArgs
		{
			public readonly object schemaRule;
			public readonly string defaultAssembly;
			public readonly string defaultNameSpace;

			public XmlNhvmRuleCoverterArgs(object schemaRule, string defaultAssembly, string defaultNameSpace)
			{
				this.schemaRule = schemaRule;
				this.defaultAssembly = defaultAssembly;
				this.defaultNameSpace = defaultNameSpace;
			}
		}
	}
}