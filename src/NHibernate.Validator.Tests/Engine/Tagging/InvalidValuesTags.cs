using System.Linq;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine.Tagging
{
	public class aEntity
	{
		[Min(Value = 20, Tags = "error")]
		[Max(Tags = new[] { "error", "warning" }, Value = 100)]
		public int ValueMinMax { get; set; }

		[Min(Value = 20, Tags = new[]{"warning", "information"})]
		public int ValueMin { get; set; }

		[Min(20)]
		[Max(100)]
		public int ValueWithoutTags { get; set; }
	}

	[TestFixture]
	public class InvalidValuesTags
	{
		[Test]
		public void WhenNoTagIsSpecified_AllTagsMatch()
		{
			// all propeties are wrong
			IClassValidator cv = new ClassValidator(typeof(aEntity));
			var invalidValues = cv.GetInvalidValues(new aEntity {ValueMinMax = 101 }).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMinMax").MatchTags.Should().Have.SameValuesAs("error", "warning");
			invalidValues.First(iv => iv.PropertyName == "ValueMin").MatchTags.Should().Have.SameValuesAs("warning", "information");
			invalidValues.First(iv => iv.PropertyName == "ValueWithoutTags").MatchTags.Should().Have.Count.EqualTo(0);
		}

		[Test]
		public void WhenTagIsSpecified_MatchTagsContainOnlyMatchs()
		{
			// only property with 'typeof(Error)' as tag
			IClassValidator cv = new ClassValidator(typeof(aEntity));
			var invalidValues = cv.GetInvalidValues(new aEntity(), new[] { "information" }).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMin").MatchTags.Should().Have.SameValuesAs("information");

			invalidValues = cv.GetInvalidValues(new aEntity { ValueMinMax = 101 }, new[] { "information", "warning" }).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMinMax").MatchTags.Should().Have.SameValuesAs("warning");
			invalidValues.First(iv => iv.PropertyName == "ValueMin").MatchTags.Should().Have.SameValuesAs("warning",
			                                                                                              "information");

			invalidValues = cv.GetInvalidValues(new aEntity(), new[] {"error", "warning"}).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMinMax").MatchTags.Should().Have.SameValuesAs("error");
			invalidValues.First(iv => iv.PropertyName == "ValueMin").MatchTags.Should().Have.SameValuesAs("warning");

			invalidValues = cv.GetInvalidValues(new aEntity { ValueMinMax = 120 }, new[] {"error", "warning"}).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMinMax").MatchTags.Should().Have.SameValuesAs("error", "warning");
			invalidValues.First(iv => iv.PropertyName == "ValueMin").MatchTags.Should().Have.SameValuesAs("warning");

			invalidValues = cv.GetInvalidValues(new aEntity(), new[]{ "error", null}).ToArray();
			invalidValues.First(iv => iv.PropertyName == "ValueMinMax").MatchTags.Should().Have.SameValuesAs("error");
			invalidValues.First(iv => iv.PropertyName == "ValueWithoutTags").MatchTags.Should().Have.Count.EqualTo(0);
		}
	}
}