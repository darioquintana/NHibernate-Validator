using System;
using System.Reflection;
using log4net;
using NHibernate.Util;

namespace NHibernate.Validator.Cfg.MappingSchema
{
	public class XmlRulesFactory
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(XmlRulesFactory));

		internal static Attribute CreateAttributeFromRule(object rule)
		{
			if (rule is NhvNotNull)
			{
				return ConvertToNotNull((NhvNotNull)rule);
			}

			if (rule is NhvNotEmpty)
			{
				return ConvertToNotEmpty((NhvNotEmpty)rule);
			}


			if (rule is NhvNotnullorempty)
			{
				return ConvertToNotNullOrEmpty((NhvNotnullorempty)rule);
			}

			if (rule is NhvLength)
			{
				return ConvertToLength((NhvLength)rule);
			}

			if (rule is NhvSize)
			{
				return ConvertToSize((NhvSize)rule);
			}

			if (rule is NhvFuture)
			{
				return ConvertToFuture((NhvFuture)rule);
			}

			if (rule is NhvPast)
			{
				return ConvertToPast((NhvPast)rule);
			}

			if (rule is NhvValid)
			{
				return ConvertToValid();
			}

			if (rule is NhvEmail)
			{
				return ConvertToEmail((NhvEmail)rule);
			}

			if (rule is NhvRange)
			{
				return ConvertToRange((NhvRange)rule);
			}

			if (rule is NhvMin)
			{
				return ConvertToMin((NhvMin)rule);
			}

			if (rule is NhvMax)
			{
				return ConvertToMax((NhvMax)rule);
			}

			if (rule is NhvAsserttrue)
			{
				return ConvertToAssertTrue((NhvAsserttrue)rule);
			}

			if (rule is NhvPattern)
			{
				return ConvertToPattern((NhvPattern)rule);
			}

			if (rule is NhvRule)
			{
				return ConvertToRule((NhvRule)rule);
			}

			if (rule is NhvIpAddress)
			{
				return ConvertToIPAddress((NhvIpAddress)rule);
			}

			if (rule is NhvDigits)
			{
				return ConvertToDigits((NhvDigits)rule);
			}

			return null;
		}

		private static Attribute ConvertToDigits(NhvDigits digitsRule)
		{
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

		private static Attribute ConvertToRule(NhvRule ruleRule)
		{
			string attribute = ruleRule.attribute;
			AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(attribute, ruleRule.@namespace, ruleRule.assembly);

			System.Type type = ReflectHelper.ClassForFullName(fullClassName.Type);
			log.Info("The type found for ruleRule = " + type.FullName);
			Attribute thisattribute = (Attribute)Activator.CreateInstance(type);
			log.Info("Attribute found = " + thisattribute);
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

		private static Attribute ConvertToRange(NhvRange rangeRule)
		{
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

		private static Attribute ConvertToPattern(NhvPattern patternRule)
		{
			log.Info("Converting to Pattern attribute");
			PatternAttribute thisAttribute = new PatternAttribute();
			thisAttribute.Regex = patternRule.regex;
			if (patternRule.message != null)
			{
				thisAttribute.Message = patternRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToIPAddress(NhvIpAddress ipAddressRule)
		{
			log.Info("Converting to IP Address attribute");
			IPAddressAttribute thisAttribute = new IPAddressAttribute();
			
			if (ipAddressRule.message != null)
			{
				thisAttribute.Message = ipAddressRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToAssertTrue(NhvAsserttrue assertTrueRule)
		{
			log.Info("Converting to AssertTrue attribute");
			AssertTrueAttribute thisAttribute = new AssertTrueAttribute();
			if (assertTrueRule.message != null)
			{
				thisAttribute.Message = assertTrueRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToMin(NhvMin minRule)
		{
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

		private static Attribute ConvertToMax(NhvMax maxRule)
		{
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

		private static Attribute ConvertToEmail(NhvEmail emailRule)
		{
			log.Info("Converting to Email attribute");
			EmailAttribute thisAttribute = new EmailAttribute();
			if (emailRule.message != null)
			{
				thisAttribute.Message = emailRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToValid()
		{
			log.Info("Converting to valid attribute");
			return new ValidAttribute();
		}

		private static Attribute ConvertToPast(NhvPast pastRule)
		{
			log.Info("Converting to Past attribute");
			PastAttribute thisAttribute = new PastAttribute();
			if (pastRule.message != null)
			{
				thisAttribute.Message = pastRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToFuture(NhvFuture futureRule)
		{
			log.Info("Converting to future attribute");
			FutureAttribute thisAttribute = new FutureAttribute();
			if (futureRule.message != null)
			{
				thisAttribute.Message = futureRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToSize(NhvSize sizeRule)
		{
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

		private static Attribute ConvertToLength(NhvLength lengthRule)
		{
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

		private static Attribute ConvertToNotEmpty(NhvNotEmpty notEmptyRule)
		{
			NotEmptyAttribute thisAttribute = new NotEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notEmptyRule.message != null)
			{
				thisAttribute.Message = notEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNullOrEmpty(NhvNotnullorempty notNullOrEmptyRule)
		{
			NotNullOrEmptyAttribute thisAttribute = new NotNullOrEmptyAttribute();
			log.Info("Converting to NotEmptyAttribute");

			if (notNullOrEmptyRule.message != null)
			{
				thisAttribute.Message = notNullOrEmptyRule.message;
			}

			return thisAttribute;
		}

		private static Attribute ConvertToNotNull(NhvNotNull notNullRule)
		{
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
	}
}