using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net.Config;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NHibernate.Validator.Tests.Integration;
using NUnit.Framework;
using System.Reflection;
using log4net.Core;
using System;
using Environment=NHibernate.Validator.Cfg.Environment;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class ValidatorEngineFixture
	{
		[Test]
		public void ConfigureNHVConfiguration()
		{
			ValidatorEngine ve = new ValidatorEngine();
			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "UseExternal";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;

			ve.Configure(nhvc);
			Assert.AreEqual(false, ve.ApplyToDDL);
			Assert.AreEqual(false, ve.AutoRegisterListeners);
			Assert.AreEqual(ValidatorMode.UseExternal, ve.DefaultMode);
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
				sw.WriteLine("<property name='default_validator_mode'>OverrideAttributeWithExternal</property>");
				sw.WriteLine("<property name='message_interpolator_class'>"
										 + typeof(PrefixMessageInterpolator).AssemblyQualifiedName + "</property>");
				sw.WriteLine("</nhv-configuration>");
				sw.Flush();
			}

			ValidatorEngine ve = new ValidatorEngine();

			ve.Configure(tmpf);
			Assert.AreEqual(false, ve.ApplyToDDL);
			Assert.AreEqual(false, ve.AutoRegisterListeners);
			Assert.AreEqual(ValidatorMode.OverrideAttributeWithExternal, ve.DefaultMode);
			Assert.IsNotNull(ve.Interpolator);
			Assert.AreEqual(typeof(PrefixMessageInterpolator), ve.Interpolator.GetType());
		}

		[Test]
		public void Configure()
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
			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "overrideattributewithExternal";
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
			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "useExternal";
			string an = Assembly.GetExecutingAssembly().FullName;
			nhvc.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Base.Address.nhv.xml"));
			nhvc.Mappings.Add(new MappingConfiguration(an, "NHibernate.Validator.Tests.Engine.DuplicatedAddress.nhv.xml"));
			using (LoggerSpy ls = new LoggerSpy("NHibernate.Validator.Engine.StateFullClassMappingFactory", Level.Warn))
			{
				ve.Configure(nhvc);
				int found =
					ls.GetOccurencesOfMessage(
						x => x.StartsWith("Duplicated external definition for class " + typeof (Address).AssemblyQualifiedName));
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
			Assert.AreEqual(0, ve.Validate(new AnyClass()).Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue<AnyClass>("aprop", new AnyClass()).Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue(new AnyClass(), "aprop").Length);

			Assert.AreEqual(0, ve.Validate("always valid").Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue("always valid", "Length").Length);
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
			[Length(3), NotNullNotEmpty]
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
			var ve = new ValidatorEngine();
			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass>("A", null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("A", null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass>("A", "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("A", "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass>("B", "123456").Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue<DerivatedClass>("B", null).Length);

			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass, string>(x => x.A, null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass, string>(x => x.A, null).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<BaseClass, string>(x => x.A, "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass, string>(x => x.A, "1234").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue<DerivatedClass, string>(x => x.B, "123456").Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue<DerivatedClass, string>(x => x.B, null).Length);

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

		[Test]
		public void ValidatePropertyValueOfInstance()
		{
			var b = new BaseClass();
			var d = new DerivatedClass();

			var ve = new ValidatorEngine();
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, "A").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "A").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, e => e.A).Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, e => e.A).Length);


			b.A = "1234";
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, "A").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(b, e => e.A).Length);
			d.A = "1234";
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "A").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, e => e.A).Length);
			d.B = "123456";
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, "B").Length);
			Assert.AreEqual(1, ve.ValidatePropertyValue(d, e => e.B).Length);
			d.B = null;
			Assert.AreEqual(0, ve.ValidatePropertyValue(d, "B").Length);
			Assert.AreEqual(0, ve.ValidatePropertyValue(d, e => e.B).Length);

			try
			{
				ve.ValidatePropertyValue(d, "WrongName");
				Assert.Fail("Intent to validate a wrong property don't throw any exception.");
			}
			catch (TargetException)
			{
				//ok
			}

			// same behavior GetInvalidValues(object)
			Assert.AreEqual(0, ve.ValidatePropertyValue(null, "A").Length);

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

		private class NoDefConstructorInterpolator:IMessageInterpolator
		{
			private NoDefConstructorInterpolator() {}

			public string Interpolate(string message, object bean, IValidator validator, IMessageInterpolator defaultInterpolator)
			{
				throw new NotImplementedException();
			}
		}
		private class WhatEver{}
		private class BadConstructorInterpolator : IMessageInterpolator
		{
			public BadConstructorInterpolator()
			{
				throw new NotImplementedException();
			}
			public string Interpolate(string message, object bean, IValidator validator, IMessageInterpolator defaultInterpolator)
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		[Test]
		public void GetInterpolator()
		{
			ValidatorEngine ve = new ValidatorEngine();
			XmlConfiguration nhvc = new XmlConfiguration();
			try
			{
				nhvc.Properties[Environment.MessageInterpolatorClass] = typeof (NoDefConstructorInterpolator).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid interpolator");
			}
			catch(ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Public constructor was not found at message interpolator: "
					+ typeof (NoDefConstructorInterpolator).AssemblyQualifiedName, e.Message);
			}

			try
			{
				nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(WhatEver).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid interpolator");
			}
			catch (ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Type does not implement '" + typeof(IMessageInterpolator).FullName + "': "
					+ typeof(WhatEver).AssemblyQualifiedName, e.Message);
			}

			try
			{
				nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(BadConstructorInterpolator).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid interpolator");
			}
			catch (ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Unable to instanciate message interpolator: "
					+ typeof(BadConstructorInterpolator).AssemblyQualifiedName, e.Message);
			}
		}

		private class NoDefConstructorLoader : IMappingLoader
		{
			private NoDefConstructorLoader() { }

			public void LoadMappings(IList<MappingConfiguration> configurationMappings)
			{
				throw new NotImplementedException();
			}

			public void AddAssembly(string assemblyName)
			{
				throw new NotImplementedException();
			}

			public void AddAssembly(Assembly assembly)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<IClassMapping> GetMappings()
			{
				throw new NotImplementedException();
			}
		}

		private class BadConstructorLoader : IMappingLoader
		{
			public BadConstructorLoader()
			{
				throw new NotImplementedException();
			}

			public void LoadMappings(IList<MappingConfiguration> configurationMappings)
			{
				throw new NotImplementedException();
			}

			public void AddAssembly(string assemblyName)
			{
				throw new NotImplementedException();
			}

			public void AddAssembly(Assembly assembly)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<IClassMapping> GetMappings()
			{
				throw new NotImplementedException();
			}
		}

		[Test]
		public void GetMappingLoader()
		{
			var ve = new ValidatorEngine();
			var nhvc = new XmlConfiguration();
			try
			{
				nhvc.Properties[Environment.MappingLoaderClass] = typeof(NoDefConstructorLoader).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid loader");
			}
			catch (ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Public constructor was not found at mapping loader: "
					+ typeof(NoDefConstructorLoader).AssemblyQualifiedName, e.Message);
			}

			try
			{
				nhvc.Properties[Environment.MappingLoaderClass] = typeof(WhatEver).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid loader");
			}
			catch (ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Type does not implement '" + typeof(IMappingLoader).FullName + "': "
					+ typeof(WhatEver).AssemblyQualifiedName, e.Message);
			}

			try
			{
				nhvc.Properties[Environment.MappingLoaderClass] = typeof(BadConstructorLoader).AssemblyQualifiedName;
				ve.Configure(nhvc);
				Assert.Fail("Expected exception for invalid loader");
			}
			catch (ValidatorConfigurationException e)
			{
				Assert.AreEqual(
					"Unable to instanciate mapping loader: "
					+ typeof(BadConstructorLoader).AssemblyQualifiedName, e.Message);
			}

			// should work
			nhvc.Properties[Environment.MappingLoaderClass] = typeof(XmlMappingLoader).AssemblyQualifiedName;
			ve.Configure(nhvc);
		}
	}
}
