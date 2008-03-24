using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.MappingSchema;
using log4net;

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

			return null;
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
			int min = int.MinValue;
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
	}
}
