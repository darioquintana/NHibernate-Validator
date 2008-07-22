using System;
using System.Collections.Generic;
using System.IO;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using NHibernate.Validator.Cfg;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class RuleAttributeFactoryFixture
	{
		[Test]
		public void CreateAttributeFromClass()
		{
			Attribute found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimal");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());
		}

		[Test, ExpectedException(typeof(InvalidAttributeNameException))]
		public void CreateAttributeFromClassWrongName()
		{
			RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "assertanimal");
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void CreateAttributeFromRule()
		{
			// Testing exception when a new element is added in XSD but not in factory
			RuleAttributeFactory.CreateAttributeFromRule("AnyObject", "", "");

			// For wellKnownRules we can't do a sure tests because we don't have a way to auto-check all
			// classes derivered from serialization.
		}

		[Test]
		public void KnownRulesConvertAssing()
		{
			NhvMapping map = MappingLoader.GetMappingFor(typeof(WellKnownRules));
			NhvmClass cm = map.@class[0];
			XmlClassMapping rm = new XmlClassMapping(cm);
			MemberInfo mi;
			List<Attribute> attributes;

			mi = typeof(WellKnownRules).GetField("AP");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual("A string value", ((ACustomAttribute)attributes[0]).Value1);
			Assert.AreEqual(123, ((ACustomAttribute)attributes[0]).Value2);
			Assert.AreEqual("custom message", ((ACustomAttribute)attributes[0]).Message);

			mi = typeof(WellKnownRules).GetField("StrProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			NotEmptyAttribute nea = FindAttribute<NotEmptyAttribute>(attributes);
			Assert.AreEqual("not-empty message", nea.Message);
			
			NotNullAttribute nna = FindAttribute<NotNullAttribute>(attributes);
			Assert.AreEqual("not-null message", nna.Message);
			
			NotNullNotEmptyAttribute nnea = FindAttribute<NotNullNotEmptyAttribute>(attributes);
			Assert.AreEqual("notnullnotempty message", nnea.Message);
			
			LengthAttribute la = FindAttribute<LengthAttribute>(attributes);
			Assert.AreEqual("length message", la.Message);
			Assert.AreEqual(1, la.Min);
			Assert.AreEqual(10, la.Max);
			
			PatternAttribute pa = FindAttribute<PatternAttribute>(attributes);
			Assert.AreEqual("pattern message", pa.Message);
			Assert.AreEqual("[0-9]+", pa.Regex);
			Assert.AreEqual(RegexOptions.Compiled, pa.Flags);

			EmailAttribute ea = FindAttribute<EmailAttribute>(attributes);
			Assert.AreEqual("email message", ea.Message);

			IPAddressAttribute ipa = FindAttribute<IPAddressAttribute>(attributes);
			Assert.AreEqual("ipAddress message", ipa.Message);

			EANAttribute enaa = FindAttribute<EANAttribute>(attributes);
			Assert.AreEqual("ean message", enaa.Message);

			CreditCardNumberAttribute ccna = FindAttribute<CreditCardNumberAttribute>(attributes);
			Assert.AreEqual("creditcardnumber message", ccna.Message);

			IBANAttribute iban = FindAttribute<IBANAttribute>(attributes);
			Assert.AreEqual("iban message", iban.Message);

			mi = typeof(WellKnownRules).GetField("DtProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			FutureAttribute fa = FindAttribute<FutureAttribute>(attributes);
			Assert.AreEqual("future message", fa.Message);
			PastAttribute psa = FindAttribute<PastAttribute>(attributes);
			Assert.AreEqual("past message", psa.Message);

			mi = typeof(WellKnownRules).GetField("DecProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			DigitsAttribute dga = FindAttribute<DigitsAttribute>(attributes);
			Assert.AreEqual("digits message", dga.Message);
			Assert.AreEqual(5, dga.IntegerDigits);
			Assert.AreEqual(2, dga.FractionalDigits);

			MinAttribute mina = FindAttribute<MinAttribute>(attributes);
			Assert.AreEqual("min message", mina.Message);
			Assert.AreEqual(100, mina.Value);

			MaxAttribute maxa = FindAttribute<MaxAttribute>(attributes);
			Assert.AreEqual("max message", maxa.Message);
			Assert.AreEqual(200, maxa.Value);

			mi = typeof(WellKnownRules).GetField("BProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			AssertTrueAttribute ata = FindAttribute<AssertTrueAttribute>(attributes);
			Assert.AreEqual("asserttrue message", ata.Message);
			AssertFalseAttribute afa = FindAttribute<AssertFalseAttribute>(attributes);
			Assert.AreEqual("assertfalse message", afa.Message);


			mi = typeof(WellKnownRules).GetField("ArrProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			SizeAttribute sa = FindAttribute<SizeAttribute>(attributes);
			Assert.AreEqual("size message", sa.Message);
			Assert.AreEqual(2, sa.Min);
			Assert.AreEqual(9, sa.Max);

			mi = typeof(WellKnownRules).GetField("Pattern");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			PatternAttribute spa = FindAttribute<PatternAttribute>(attributes);
			Assert.AreEqual("{validator.pattern}", spa.Message);
			Assert.AreEqual(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", spa.Regex);
			Assert.AreEqual(RegexOptions.CultureInvariant | RegexOptions.IgnoreCase, spa.Flags);
		}

		private static T FindAttribute<T>(List<Attribute> attr) where T : Attribute
		{
			return (T)attr.Find(delegate(Attribute a)
														{ return a is T; });
		}

		[Test, ExpectedException(typeof(InvalidPropertyNameException))]
		public void WrongPropertyInCustomAttribute()
		{
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-mapping xmlns='urn:nhibernate-validator-1.0'");
				sw.WriteLine(" assembly='NHibernate.Validator.Tests'");
				sw.WriteLine(" namespace='NHibernate.Validator.Tests.Configuration'>");
				sw.WriteLine("<class name='WellKnownRules'>");
				sw.WriteLine("<property name='AP'>");
				sw.WriteLine("<rule attribute='ACustomAttribute'>");
				sw.WriteLine("<param name='WrongName' value='A string value'/>");
				sw.WriteLine("</rule>");
				sw.WriteLine("</property>");
				sw.WriteLine("</class>");
				sw.WriteLine("</nhv-mapping>"); 
				sw.Flush();
			}

			MappingLoader ml = new MappingLoader();
			using (StreamReader sr = new StreamReader(tmpf))
			{
				ml.AddInputStream(sr.BaseStream, tmpf);
			}
			NhvMapping map = ml.Mappings[0];
			NhvmClass cm = map.@class[0];
			XmlClassMapping rm = new XmlClassMapping(cm);
			MemberInfo mi;

			mi = typeof(WellKnownRules).GetField("AP");
			rm.GetMemberAttributes(mi);
		}

		[Test]
		public void SingleRegexOptionsParsing()
		{
			Assert.AreEqual(RegexOptions.Compiled, RuleAttributeFactory.ParsePatternSingleFlags("cOmPiLed"));
			Assert.AreEqual(RegexOptions.CultureInvariant, RuleAttributeFactory.ParsePatternSingleFlags("CultureInvariant"));
			Assert.AreEqual(RegexOptions.ECMAScript, RuleAttributeFactory.ParsePatternSingleFlags("ECMAScript"));
			Assert.AreEqual(RegexOptions.ExplicitCapture, RuleAttributeFactory.ParsePatternSingleFlags("ExplicitCapture"));
			Assert.AreEqual(RegexOptions.IgnoreCase, RuleAttributeFactory.ParsePatternSingleFlags("IgnoreCase"));
			Assert.AreEqual(RegexOptions.IgnorePatternWhitespace, RuleAttributeFactory.ParsePatternSingleFlags("IgnorePatternWhitespace"));
			Assert.AreEqual(RegexOptions.Multiline, RuleAttributeFactory.ParsePatternSingleFlags("Multiline"));
			Assert.AreEqual(RegexOptions.None, RuleAttributeFactory.ParsePatternSingleFlags("None"));
			Assert.AreEqual(RegexOptions.RightToLeft, RuleAttributeFactory.ParsePatternSingleFlags("RightToLeft"));
			Assert.AreEqual(RegexOptions.Singleline, RuleAttributeFactory.ParsePatternSingleFlags("Singleline"));
			try
			{
				RuleAttributeFactory.ParsePatternSingleFlags("Pizza");
			}
			catch(ValidatorConfigurationException)
			{
				// Ok
			}
		}

		[Test]
		public void RegexOptionsParsing()
		{
			Assert.AreEqual(RegexOptions.Compiled | RegexOptions.CultureInvariant,
			                RuleAttributeFactory.ParsePatternFlags("cOmPiLed | CultureInvariant"));
			Assert.AreEqual(RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase,
			                RuleAttributeFactory.ParsePatternFlags("Compiled|IgnoreCase|IgnorePatternWhitespace"));

			// Ignore strange user ;)
			Assert.AreEqual(RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace,
			                RuleAttributeFactory.ParsePatternFlags("Compiled||  | |IgnorePatternWhitespace"));
		}
	}
}