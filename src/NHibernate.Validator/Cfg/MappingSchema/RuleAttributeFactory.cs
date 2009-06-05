using System;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using NHibernate.Util;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
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
			wellKnownRules[typeof(NhvmNotnullNotempty)] = ConvertToNotNullNotEmpty;
			wellKnownRules[typeof(NhvmLength)] = ConvertToLength;
			wellKnownRules[typeof(NhvmSize)] = ConvertToSize;
			wellKnownRules[typeof(NhvmFuture)] = ConvertToFuture;
			wellKnownRules[typeof(NhvmPast)] = ConvertToPast;
			wellKnownRules[typeof(NhvmEmail)] = ConvertToEmail;
			wellKnownRules[typeof(NhvmRange)] = ConvertToRange;
			wellKnownRules[typeof(NhvmMin)] = ConvertToMin;
			wellKnownRules[typeof(NhvmMax)] = ConvertToMax;
			wellKnownRules[typeof(NhvmAsserttrue)] = ConvertToAssertTrue;
			wellKnownRules[typeof(NhvmAssertfalse)] = ConvertToAssertFalse;
			wellKnownRules[typeof(NhvmPattern)] = ConvertToPattern;
			wellKnownRules[typeof(NhvmRule)] = ConvertToRule;
			wellKnownRules[typeof(NhvmIpaddress)] = ConvertToIPAddress;
			wellKnownRules[typeof(NhvmDigits)] = ConvertToDigits;
			wellKnownRules[typeof(NhvmCreditcardnumber)] = ConvertToCreditCardNumber;
			wellKnownRules[typeof(NhvmEan)] = ConvertToEAN;
			wellKnownRules[typeof(NhvmFileexists)] = ConvertToFileExists;
			wellKnownRules[typeof(NhvmValid)] = ConvertToValid;
			wellKnownRules[typeof(NhvmIban)] = ConvertToIBAN;
		}

		public static Attribute CreateAttributeFromRule(object rule, string defaultAssembly, string defaultNameSpace)
		{
			// public Only for test scope
			ConvertSchemaRule converter;
			if (wellKnownRules.TryGetValue(rule.GetType(), out converter))
				return converter(new XmlNhvmRuleConverterArgs(rule, defaultAssembly, defaultNameSpace));
			else
				throw new ValidatorConfigurationException("Unrecognized XML element:" + rule.GetType());
		}

		/// <summary>
		/// Create the attribute of a entity-validator from XML definitions.
		/// </summary>
		/// <param name="entityType">The entity class where associate the attribute.</param>
		/// <param name="attributename">The attribute name in the mapping.</param>
		/// <returns>The <see cref="Attribute"/> instance.</returns>
		/// <remarks>
		/// We are using the conventions:
		/// - The attribute must be defined in the same namespace of the <paramref name="entityType"/>.
		/// - The attribute class may have the postfix "Attribute" without need to use it in the mapping.
		/// </remarks>
		public static Attribute CreateAttributeFromClass(System.Type entityType, NhvmClassAttributename attributename)
		{
			// public Only for test scope
			Assembly assembly = entityType.Assembly;
			System.Type type = assembly.GetType(entityType.Namespace + "." + GetText(attributename) + "Attribute");

			if (type == null)
			{
				type = assembly.GetType(entityType.Namespace + "." + GetText(attributename));
			}

			if (type == null)
			{
				throw new InvalidAttributeNameException(GetText(attributename), entityType);
			}

			Attribute attribute = (Attribute)Activator.CreateInstance(type);
			if (attribute is IRuleArgs)
			{
				((IRuleArgs) attribute).Message = attributename.message;
			}
			return attribute;
		}

		private static string GetText(NhvmClassAttributename attributename)
		{
			string[] text = attributename.Text;
			if (text != null)
			{
				string result = string.Join(System.Environment.NewLine, text).Trim();
				return result.Length == 0 ? null : result;
			}
			else
				return null;
		}

		private static Attribute ConvertToDigits(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToValid(XmlNhvmRuleConverterArgs rule)
		{
			ValidAttribute validAttribute = new ValidAttribute();
			return validAttribute;
		}

		private static Attribute ConvertToRule(XmlNhvmRuleConverterArgs rule)
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
					object value;
					if (!(propInfo.PropertyType is string))
					{
						value = Convert.ChangeType(parameter.value, propInfo.PropertyType);
					}
					else
						value = parameter.value;
					propInfo.SetValue(thisattribute, value, null);
				}
				else
				{
					throw new InvalidPropertyNameException(
						string.Format("The custom attribute '{0}' don't have the property '{1}'; Check for typo.", type.FullName, parameter.name), parameter.name,
						type);
				}
			}

			return thisattribute;
		}

		private static Attribute ConvertToRange(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToPattern(XmlNhvmRuleConverterArgs rule)
		{
			NhvmPattern patternRule = (NhvmPattern)rule.schemaRule;

			log.Info("Converting to Pattern attribute");
			PatternAttribute thisAttribute = new PatternAttribute();
			thisAttribute.Regex = patternRule.regex;
			if (!string.IsNullOrEmpty(patternRule.regexoptions))
			{
				thisAttribute.Flags = ParsePatternFlags(patternRule.regexoptions);
			}
			if (patternRule.message != null)
			{
				thisAttribute.Message = patternRule.message;
			}

			return thisAttribute;
		}

		public static RegexOptions ParsePatternFlags(string xmlValue)
		{
			RegexOptions result = RegexOptions.None;
			string[] tokens = xmlValue.Split(new char[] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string token in tokens)
			{
				result |= ParsePatternSingleFlags(token);
			}
			return result;
		}

		public static RegexOptions ParsePatternSingleFlags(string xmlValue)
		{
			switch (xmlValue.ToLowerInvariant())
			{
				case "compiled":
					return RegexOptions.Compiled;
				case "cultureinvariant":
					return RegexOptions.CultureInvariant;
				case "ecmascript":
					return RegexOptions.ECMAScript;
				case "explicitcapture":
					return RegexOptions.ExplicitCapture;
				case "ignorecase":
					return RegexOptions.IgnoreCase;
				case "ignorepatternwhitespace":
					return RegexOptions.IgnorePatternWhitespace;
				case "multiline":
					return RegexOptions.Multiline;
				case "none":
					return RegexOptions.None;
				case "righttoleft":
					return RegexOptions.RightToLeft;
				case "singleline":
					return RegexOptions.Singleline;
				default:
					throw new ValidatorConfigurationException(
						string.Format(
							"Invalid value for regex-options: '{0}' ; see documentation of System.Text.RegularExpressions.RegexOptions for valid values.",
							xmlValue));
			}
		}

		private static Attribute ConvertToIPAddress(XmlNhvmRuleConverterArgs rule)
		{
			NhvmIpaddress ipAddressRule = (NhvmIpaddress)rule.schemaRule;
			log.Info("Converting to IP Address attribute");
			IPAddressAttribute thisAttribute = new IPAddressAttribute();

			if (ipAddressRule.message != null)
			{
				thisAttribute.Message = ipAddressRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToAssertTrue(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToAssertFalse(XmlNhvmRuleConverterArgs rule)
		{
			NhvmAssertfalse assertTrueRule = (NhvmAssertfalse)rule.schemaRule;

			log.Info("Converting to AssertFalse attribute");
			AssertFalseAttribute thisAttribute = new AssertFalseAttribute();
			if (assertTrueRule.message != null)
			{
				thisAttribute.Message = assertTrueRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMin(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToMax(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToEmail(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToPast(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToFuture(XmlNhvmRuleConverterArgs rule)
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


		private static Attribute ConvertToSize(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToLength(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToNotEmpty(XmlNhvmRuleConverterArgs rule)
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

		private static Attribute ConvertToNotNullNotEmpty(XmlNhvmRuleConverterArgs rule)
		{
			NhvmNotnullNotempty notNullOrEmptyRule = (NhvmNotnullNotempty)rule.schemaRule;
			NotNullNotEmptyAttribute thisAttribute = new NotNullNotEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notNullOrEmptyRule.message != null)
			{
				thisAttribute.Message = notNullOrEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToCreditCardNumber(XmlNhvmRuleConverterArgs rule)
		{
			NhvmCreditcardnumber creditCardNumberRule = (NhvmCreditcardnumber)rule.schemaRule;
			CreditCardNumberAttribute thisAttribute = new CreditCardNumberAttribute();
			log.Info("Converting to CreditCardNumberAttribute");

			if (creditCardNumberRule.message != null)
			{
				thisAttribute.Message = creditCardNumberRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToEAN(XmlNhvmRuleConverterArgs rule)
		{
			NhvmEan eanRule = (NhvmEan)rule.schemaRule;
			EANAttribute thisAttribute = new EANAttribute();
			log.Info("Converting to EANAttribute");

			if (eanRule.message != null)
			{
				thisAttribute.Message = eanRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNull(XmlNhvmRuleConverterArgs rule)
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


		private static Attribute ConvertToFileExists(XmlNhvmRuleConverterArgs rule)
		{
			NhvmFileexists fileExistsRule = (NhvmFileexists)rule.schemaRule;
			log.Info("Converting to file exists attribute");
			FileExistsAttribute thisAttribute = new FileExistsAttribute();
			if (fileExistsRule.message != null)
			{
				thisAttribute.Message = fileExistsRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToIBAN(XmlNhvmRuleConverterArgs rule)
		{
			NhvmIban ibanRule = (NhvmIban)rule.schemaRule;
			log.Info("Converting to IBAN attribute");
			IBANAttribute thisAttribute = new IBANAttribute();

			if (ibanRule.message != null)
			{
				thisAttribute.Message = ibanRule.message;
			}

			return thisAttribute;
		}

		private delegate Attribute ConvertSchemaRule(XmlNhvmRuleConverterArgs schemaRule);
		private class XmlNhvmRuleConverterArgs
		{
			public readonly object schemaRule;
			public readonly string defaultAssembly;
			public readonly string defaultNameSpace;

			public XmlNhvmRuleConverterArgs(object schemaRule, string defaultAssembly, string defaultNameSpace)
			{
				this.schemaRule = schemaRule;
				this.defaultAssembly = defaultAssembly;
				this.defaultNameSpace = defaultNameSpace;
			}
		}
	}
}