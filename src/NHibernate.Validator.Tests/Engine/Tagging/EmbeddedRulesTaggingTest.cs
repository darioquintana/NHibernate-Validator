using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	[TestFixture]
	public class EmbeddedRulesTaggingTest
	{
		public IEnumerable<System.Type> Rules
		{
			get
			{
				return
					typeof (IRuleArgs).Assembly.GetTypes().Where(t => typeof (IRuleArgs).IsAssignableFrom(t) && typeof (IRuleArgs) != t);
			}
		}

		[Test]
		public void AllRuleArgs_SupportsTags([ValueSource("Rules")]System.Type attribute)
		{
			typeof (ITagableRule).Should().Be.AssignableFrom(attribute);
		}
	}
}