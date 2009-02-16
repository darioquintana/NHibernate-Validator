using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Configuration.Loquacious;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class IsNumericValidatorFixture
	{
		[Test]
		public void IsValid([Values("0","123","12345.6")] 
			string value)
		{
			var v = new IsNumericValidator();
			Assert.That(v.IsValid(value));
		}

		[Test]
		public void IsNotValid([Values("a", "123.,", "12345...6")] 
			string value)
		{
			var v = new IsNumericValidator();
			Assert.That(!v.IsValid(value));
		}

		[Test]
		public void Attributes()
		{
			var v = new ReflectionClassMapping(typeof(MySample));
			PropertyInfo lpi = typeof(MySample).GetProperty("Str");
			var ma = v.GetMemberAttributes(lpi);
			Assert.That(ma.Count(), Is.EqualTo(1));
			Assert.That(ma.First(), Is.InstanceOf<IsNumericAttribute>());
			Assert.That(ma.OfType<IsNumericAttribute>().First().Message, Is.EqualTo("Must be a number"));
		}

		[Test]
		public void Loquacious()
		{
			var v = new ValidationDef<KnownRules>();
			v.Define(x => x.StrProp).IsNumeric();
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			PropertyInfo lpi = typeof(KnownRules).GetProperty("StrProp");

			Assert.That(cm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(cm.GetMemberAttributes(lpi).First(), Is.InstanceOf<IsNumericAttribute>());
		}

		public class MySample
		{
			[IsNumeric(Message = "Must be a number")]
			public string Str { get; set; }
		}
	}
}