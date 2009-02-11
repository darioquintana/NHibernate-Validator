using System.Linq;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Mappings;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Mappings
{
	[TestFixture]
	public class OpenClassMappingFixture
	{
		private const BindingFlags membersBindingFlags =
			BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			| BindingFlags.Static;

		[Test]
		public void EntityType()
		{
			var ocm = new OpenClassMapping<CleanAddress>();
			Assert.That(ocm.EntityType, Is.EqualTo(typeof(CleanAddress)));
		}

		[Test]
		public void AddBeanValidator()
		{
			var ocm = new OpenClassMapping<CleanAddress>();
			Assert.That(ocm.GetClassAttributes().Count(), Is.EqualTo(0));
			ocm.AddBeanValidator(new AssertAnimalAttribute());
			Assert.That(ocm.GetClassAttributes().Count(), Is.EqualTo(1));
			ocm.AddBeanValidator(new OtherValidatorAttribute());
			Assert.That(ocm.GetClassAttributes().Count(), Is.EqualTo(2));
		}

		[Test]
		public void AddFieldConstraint()
		{
			var ocm = new OpenClassMapping<CleanAddress>();
			FieldInfo fpi = typeof(CleanAddress).GetField("blacklistedZipCode", membersBindingFlags);
			ocm.AddConstraint(fpi, new NotNullAttribute());
			Assert.That(ocm.GetMembers().Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(fpi).Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(fpi).First(), Is.InstanceOf<NotNullAttribute>());
			FieldInfo fpi1 = typeof(CleanAddress).GetField("blacklistedZipCode", membersBindingFlags);
			ocm.AddConstraint(fpi, new NotEmptyAttribute());
			Assert.That(ocm.GetMembers().Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(fpi1).Count(), Is.EqualTo(2));
			Assert.That(ocm.GetMemberAttributes(fpi1).ElementAt(1), Is.InstanceOf<NotEmptyAttribute>());
			FieldInfo spi = typeof(CleanAddress).GetField("floor", membersBindingFlags);
			Assert.That(ocm.GetMemberAttributes(spi).Count(), Is.EqualTo(0));
		}

		[Test]
		public void AddPropertyConstraint()
		{
			var ocm = new OpenClassMapping<CleanAddress>();
			PropertyInfo lpi = typeof(CleanAddress).GetProperty("Line1", membersBindingFlags);
			ocm.AddConstraint(lpi, new NotNullAttribute());
			Assert.That(ocm.GetMembers().Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(lpi).First(), Is.InstanceOf<NotNullAttribute>());
			PropertyInfo lpi1 = typeof(CleanAddress).GetProperty("Line1", membersBindingFlags);
			ocm.AddConstraint(lpi, new NotEmptyAttribute());
			Assert.That(ocm.GetMembers().Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(lpi1).Count(), Is.EqualTo(2));
			Assert.That(ocm.GetMemberAttributes(lpi1).ElementAt(1), Is.InstanceOf<NotEmptyAttribute>());
			PropertyInfo spi = typeof(CleanAddress).GetProperty("State", membersBindingFlags);
			Assert.That(ocm.GetMemberAttributes(spi).Count(), Is.EqualTo(0));
		}

		[Test]
		public void MixingPropertiesAndFields()
		{
			var ocm = new OpenClassMapping<CleanAddress>();
			PropertyInfo lpi = typeof(CleanAddress).GetProperty("Line1", membersBindingFlags);
			ocm.AddConstraint(lpi, new NotNullAttribute());
			FieldInfo fpi = typeof(CleanAddress).GetField("blacklistedZipCode", membersBindingFlags);
			ocm.AddConstraint(fpi, new NotNullAttribute());

			Assert.That(ocm.GetMembers().Count(), Is.EqualTo(2));
			Assert.That(ocm.GetMemberAttributes(lpi).Count(), Is.EqualTo(1));
			Assert.That(ocm.GetMemberAttributes(fpi).Count(), Is.EqualTo(1));
		}
	}
}