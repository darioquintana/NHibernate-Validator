using System;
using System.Collections.Generic;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Integration;
using NUnit.Framework;
using Environment=NHibernate.Validator.Engine.Environment;
using System.IO;

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

	}
}
