using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.EmbeddedResources
{
	public class ValidatorsMessagesTest
	{
		[Test]
		public void AllMessageTemplatesExistsInResource()
		{
			ResourceManager manager = new ResourceManager("Nhibernate.Validator.Properties.DefaultValidatorMessages", typeof(IRuleArgs).Assembly);

			var incompleteTypes = GetResourceFileIncompleteValidators(manager).ToList();

			incompleteTypes.Should().Be.Empty();
		}

		private IEnumerable<string> GetResourceFileIncompleteValidators(ResourceManager manager)
		{
			return GetResourceFileValidatorsMessages(manager).Where(message => string.IsNullOrEmpty(message));
		}

		private IEnumerable<string> GetResourceFileValidatorsMessages(ResourceManager manager)
		{
			Assembly a = typeof(IRuleArgs).Assembly;
			foreach (System.Type type in a.GetTypes())
			{
				if (!type.IsAbstract &&
						typeof(IRuleArgs).IsAssignableFrom(type) &&
						type.GetConstructor(new System.Type[0]) != null)
				{
					IRuleArgs item = (IRuleArgs)Activator.CreateInstance(type, null, null);
					if (string.IsNullOrEmpty(item.Message) || item.Message.Length < 2)
					{
						continue;
					}

					string resName = item.Message.Substring(1, item.Message.Length - 2);
					yield return manager.GetString(resName);
				}
			}
		}

		[Test]
		public void ShowIncompleteCultures()
		{
			var manager = new ResourceManager("Nhibernate.Validator.Properties.DefaultValidatorMessages", typeof (IRuleArgs).Assembly);
			// this is not a real test because we can't maintain all files
			// Users can run this "test" and check what is wrong in the resources they need, then a nice patch is wellcome
			var embeddedCultures = new[] {"it", "es", "de", "fr", "hr", "lv", "nl", "pl"};
			foreach (var embeddedCulture in embeddedCultures)
			{
				Console.WriteLine("For Culture:" + embeddedCulture);
				using (new WithUiCulture(embeddedCulture))
				{
					foreach (var validatorName in GetResourceFileValidatorsMessages(manager))
					{
						Console.WriteLine("    " + validatorName);
					}
				}
			}
		}

		private class WithUiCulture : IDisposable
		{
			private readonly string oldCulture;

			public WithUiCulture(string culture)
			{
				oldCulture = CultureInfo.CurrentUICulture.Name;
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture, false);
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(oldCulture, false);
			}

			#endregion
		}
	}
}