using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	[TestFixture]
	public class EmbeddedRulesTaggingTest
	{
		public static IEnumerable<System.Type> Rules
		{
			get
			{
				return
					typeof (IRuleArgs).Assembly.GetTypes().Where(t => typeof (IRuleArgs).IsAssignableFrom(t) && typeof (IRuleArgs) != t && typeof(ValidAttribute) != t);
			}
		}

		[Test]
		public void AllRuleArgs_SupportsTags([ValueSource(nameof(Rules))]System.Type attribute)
		{
			typeof (ITagableRule).Should().Be.AssignableFrom(attribute);
		}
	}
}