using System.IO;
using System.Xml;
using log4net.Config;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;
using NHibernate.Validator.Tests.Integration;
using NUnit.Framework;
using System.Reflection;
using log4net.Core;
using System;
using Environment=NHibernate.Validator.Engine.Environment;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class ValidatorEngineFixture
	{
		[Test]
		public void ConfigureNHVConfiguration()
		{
			ValidatorEngine ve = new ValidatorEngine();
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "UseXML";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;

			ve.Configure(nhvc);
			Assert.AreEqual(false, ve.ApplyToDDL);
			Assert.AreEqual(false, ve.AutoRegisterListeners);
			Assert.AreEqual(ValidatorMode.UseXml, ve.DefaultMode);
			Assert.IsNotNull(ve.Interpolator);
			Assert.AreEqual(typeof(PrefixMessageInterpolator), ve.Interpolator.GetType());
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void IntentWrongNHVConfig()
		{
			ValidatorEngine ve = new ValidatorEngine();
			ve.Configure((INHVConfiguration)null);
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void IntentWrongXmlConfig()
		{
			ValidatorEngine ve = new ValidatorEngine();
			ve.Configure((XmlReader)null);
		}

		[Test]
		public void ConfigureFile()
		{
			// This test is for .Configure(XmlReader) too
			string tmpf = Path.GetTempFileName();
			using (StreamWriter sw = new StreamWriter(tmpf))
			{
				sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
				sw.WriteLine("<nhv-configuration xmlns='urn:nhv-configuration-1.0'>");
				sw.WriteLine("<property name='apply_to_ddl'>false</property>");
				sw.WriteLine("<property name='autoregister_listeners'>false</property>");
				sw.WriteLine("<property name='default-validator-mode'>OverrideAttributeWithXml</property>");
				sw.WriteLine("<property name='message_interpolator_class'>"
										 + typeof(PrefixMessageInterpolator).AssemblyQualifiedName + "</property>");
				sw.WriteLine("</nhv-configuration>");
				sw.Flush();
			}

			ValidatorEngine ve = new ValidatorEngine();

			ve.Configure(tmpf);
			Assert.AreEqual(false, ve.ApplyToDDL);
			Assert.AreEqual(false, ve.AutoRegisterListeners);
			Assert.AreEqual(ValidatorMode.OverrideAttributeWithXml, ve.DefaultMode);
			Assert.IsNotNull(ve.Interpolator);
			Assert.AreEqual(typeof(PrefixMessageInterpolator), ve.Interpolator.GetType());
		}

		[Test]
		public void ConfigureAppConfig()
		{
			ValidatorEngine ve = new ValidatorEngine();

			ve.Configure();
			Assert.AreEqual(true, ve.ApplyToDDL);
			Assert.AreEqual(true, ve.AutoRegisterListeners);
			Assert.AreEqual(ValidatorMode.UseAttribute, ve.DefaultMode);
			Assert.IsNotNull(ve.Interpolator);
			Assert.AreEqual(typeof(PrefixMessageInterpolator), ve.Interpolator.GetType());
		}

		[Test]
		public void InitializeValidators()
		{
			ValidatorEngine ve = new ValidatorEngine();
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "overrideattributewithxml";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			string an = Assembly.GetExecutingAssembly().FullName;
			nhvc.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Base.Address.nhv.xml"));
			ve.Configure(nhvc);
			Assert.IsNotNull(ve.GetValidator<Address>());

			Assert.IsNull(ve.GetValidator<Boo>());
			// Validate something and then its validator is initialized
			Boo b = new Boo();
			ve.IsValid(b);
			Assert.IsNotNull(ve.GetValidator<Boo>());
		}

		[Test]
		public void DuplicateClassDef()
		{
			XmlConfigurator.Configure();
			ValidatorEngine ve = new ValidatorEngine();
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "usexml";
			string an = Assembly.GetExecutingAssembly().FullName;
			nhvc.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Base.Address.nhv.xml"));
			nhvc.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Engine.DuplicatedAddress.nhv.xml"));
			using (LoggerSpy ls = new LoggerSpy(typeof(ValidatorEngine), Level.Warn))
			{
				ve.Configure(nhvc);
				int found = 0;
				foreach (LoggingEvent loggingEvent in ls.Appender.GetEvents())
				{
					if (loggingEvent.RenderedMessage.Contains("Duplicated XML definition for class " + typeof(Address).AssemblyQualifiedName))
						found++;
				}
				Assert.AreEqual(1, found);
			}
		}

		[Test]
		public void NullIsAllwaysValid()
		{
			ValidatorEngine ve = new ValidatorEngine();
			Assert.IsTrue(ve.IsValid(null));
			Assert.AreEqual(0, ve.Validate(null).Length);
			ve.AssertValid(null); // not cause exception
		}

		[Test]
		public void AddValidatorWithoutUseIt()
		{
			ValidatorEngine ve = new ValidatorEngine();
			ve.AddValidator<Boo>();
			Assert.IsNotNull(ve.GetValidator<Boo>());
		}

		private class AnyClass
		{
			public int aprop;
		}
		[Test]
		public void ValidateAnyClass()
		{
			ValidatorEngine ve = new ValidatorEngine();
			Assert.IsTrue(ve.IsValid(new AnyClass()));
			Assert.IsNotNull(ve.GetValidator<AnyClass>());
			ve.AssertValid(new AnyClass()); // not cause exception
			Assert.AreEqual(0,ve.Validate(new AnyClass()).Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue<AnyClass>("aprop", new AnyClass()).Length);
		}

		[Test]
		public void AssertValid()
		{
			ValidatorEngine ve = new ValidatorEngine();
			ve.AssertValid(new AnyClass()); // not cause exception
			try
			{
				Boo b = new Boo();
				ve.AssertValid(b);
				Assert.Fail("An invalid bean not throw exception.");
			}
			catch (InvalidStateException)
			{
				// OK
			}
		}

		public class BaseClass
		{
			[Length(3), NotNullOrEmpty]
			public string A;
		}

		public class DerivatedClass : BaseClass
		{
			[Length(5)]
			public string B;
		}

		[Test]
		public void ValidatePropertyValue()
		{
			ValidatorEngine ve = new ValidatorEngine();
			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass>("A", null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("A", null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass>("A", "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("A", "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("B", "123456").Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue<DerivatedClass>("B", null).Length);

			try
			{
				ve.ValidatePropertyValue<DerivatedClass>("WrongName", null);
				Assert.Fail("Intent to validate a wrong property don't throw any exception.");
			}
			catch (TargetException)
			{
				//ok
			}
		}

		[Test, Ignore("Not implemented yet.")]
		public void ValidatePropertyValueOfInstance()
		{
			BaseClass b = new BaseClass();
			DerivatedClass d = new DerivatedClass();

			ValidatorEngine ve = new ValidatorEngine();
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, "A").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "A").Length);

			b.A = "1234";
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, "A").Length);
			d.A = "1234";
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "A").Length);
			d.B = "123456";
			Assert.AreEqual(2, ve.ValidatePropertyValue(d, "B").Length);
			d.B = null;
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "B").Length);

			try
			{
				ve.ValidatePropertyValue(d, "WrongName");
				Assert.Fail("Intent to validate a wrong property don't throw any exception.");
			}
			catch (TargetException)
			{
				//ok
			}

			try
			{
				ve.ValidatePropertyValue(null, "A");
				Assert.Fail("Intent to validate a null instance don't throw any exception.");
			}
			catch (ArgumentNullException)
			{
				//ok
			}


			try
			{
				ve.ValidatePropertyValue(d, "");
				Assert.Fail("Intent to validate a empty property name don't throw any exception.");
			}
			catch (ArgumentNullException)
			{
				//ok
			}
		}
	}
}
