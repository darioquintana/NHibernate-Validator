#if !NETFX
using System.Configuration;
using System.IO;
using System.Reflection;
#endif
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
#if !NETFX
using NHibernate.Cfg;
using NHibernate.Cfg.ConfigurationSchema;
#endif
using NUnit.Framework;

namespace NHibernate.Validator.Tests
{
	[SetUpFixture]
	public class TestsContext
	{
		public static void AssumeSystemTypeIsSerializable() =>
			Assume.That(typeof(System.Type).IsSerializable, Is.True);

#if !NETFX
		private static bool ExecutingWithVsTest { get; } =
			Assembly.GetEntryAssembly()?.GetName().Name == "testhost";

		private static bool _removeTesthostConfig;
#endif

		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
#if !NETFX
			//When .NET Core App 2.0 tests run from VS/VSTest the entry assembly is "testhost.dll"
			//so we need to explicitly load the configuration
			if (ExecutingWithVsTest)
			{
				var assemblyPath =
					Path.Combine(TestContext.CurrentContext.TestDirectory, Path.GetFileName(typeof(TestsContext).Assembly.Location));
				Environment.InitializeGlobalProperties(GetTestAssemblyHibernateConfiguration(assemblyPath));
				ReadValidatorSectionFromTesthostConfig(assemblyPath, "nhv-configuration");
			}
#endif
			ConfigureLog4Net();
		}

#if !NETFX
		[OneTimeTearDown]
		public void RunAfterAnyTests()
		{
			if (_removeTesthostConfig)
			{
				File.Delete(GetTesthostConfigPath());
			}
		}

		private static void ReadValidatorSectionFromTesthostConfig(string assemblyPath, string configSectionName)
		{
			// For caches section, ConfigurationManager being directly used, the only workaround is to provide
			// the configuration with its expected file name...
			var configPath = assemblyPath + ".config";
			// If this copy fails: either testconfig has started having its own file, and this hack can no more be used,
			// or a previous test run was interupted before its cleanup (RunAfterAnyTests): go clean it manually.
			// Discussion about this mess: https://github.com/dotnet/corefx/issues/22101
			File.Copy(configPath, GetTesthostConfigPath());
			_removeTesthostConfig = true;
			ConfigurationManager.RefreshSection(configSectionName);
		}

		private static string GetTesthostConfigPath()
		{
			return Assembly.GetEntryAssembly().Location + ".config";
		}

		private static IHibernateConfiguration GetTestAssemblyHibernateConfiguration(string assemblyPath)
		{
			var configuration = ConfigurationManager.OpenExeConfiguration(assemblyPath);
			var section = configuration.GetSection(CfgXmlHelper.CfgSectionName);
			return HibernateConfiguration.FromAppConfig(section.SectionInformation.GetRawXml());
		}
#endif

		private static void ConfigureLog4Net()
		{
			var hierarchy = (Hierarchy)LogManager.GetRepository(typeof(TestsContext).Assembly);

			var consoleAppender = new ConsoleAppender
			{
				Layout = new PatternLayout("%d{ABSOLUTE} %-5p %c{1}:%L - %m%n"),
			};
			hierarchy.Root.Level = log4net.Core.Level.Warn;
			hierarchy.Root.AddAppender(consoleAppender);
			hierarchy.Configured = true;
#if !NETFX
#pragma warning disable 618
			LoggerProvider.SetLoggersFactory(new Log4NetLoggerFactory());
#pragma warning restore 618
#endif
		}
	}
}
