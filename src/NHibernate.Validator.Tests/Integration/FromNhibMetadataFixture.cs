using System;
using System.Collections;
using System.Linq;
using NHibernate.Mapping;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Integration
{
	[TestFixture]
	public class FromNhibMetadataFixture : PersistenceTest
	{
		protected override IList Mappings => new[] { "Integration.FromNhibMetadata.hbm.xml" };

		private ISharedEngineProvider _fortest;

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			// The ValidatorInitializer and the ValidateEventListener share the same engine

			// Initialize the SharedEngine
			_fortest = new NHibernateSharedEngineProvider();
			Cfg.Environment.SharedEngineProvider = _fortest;
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			ve.Clear();
			var nhvc = new XmlConfiguration
			{
				Properties =
				{
					[Cfg.Environment.ApplyToDDL] = "true",
					[Cfg.Environment.AutoGenerateFromMapping] = "true",
					[Cfg.Environment.AutoregisterListeners] = "true",
					[Cfg.Environment.ValidatorMode] = "UseAttribute",
					[Cfg.Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName
				}
			};

			ve.Configure(nhvc);
			//ve.IsValid(new HibernateAnnotationIntegrationFixture.AnyClass());// add the element to engine for test

			configuration.Initialize();
		}

		protected override void OnTestFixtureTearDown()
		{
			// reset the engine
			Cfg.Environment.SharedEngineProvider = null;
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();

			using (var s = OpenSession())
			using (var txn = s.BeginTransaction())
			{
				s.Delete("from FromNhibMetadata");
				txn.Commit();
			}
		}

		[Test]
		public void ApplyFromStringColumn()
		{
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var vl = ve.GetValidator<FromNhibMetadata>();

			Assert.IsTrue(vl.HasValidationRules, "Validation rules must be created from NHib metadata");

			var sva = vl.GetMemberConstraints("StrValue").FirstOrDefault();

			Assert.That(sva, Is.Not.Null);
			Assert.That(
				sva,
				Is.InstanceOf<LengthAttribute>(),
				"LengthAttribute should be generated from Nhib metadata for StrValue property");

			var sval = (LengthAttribute) sva;

			Assert.That(sval.Max, Is.EqualTo(5));
		}

		[Test]
		public void ApplyFromDateNotNullColumn()
		{
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var vl = ve.GetValidator<FromNhibMetadata>();

			var sva = vl.GetMemberConstraints("DateNotNull").FirstOrDefault();

			Assert.That(
				sva,
				Is.InstanceOf<NotNullAttribute>(),
				"NotNullAttribute should be generated from NHib metadata for DateNotNull property");
		}

		[Test]
		public void ApplyFromDecimalColumn()
		{
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var vl = ve.GetValidator<FromNhibMetadata>();

			var sva = vl.GetMemberConstraints("Dec").FirstOrDefault();

			Assert.That(sva, Is.Not.Null);
			Assert.That(
				sva,
				Is.InstanceOf<DigitsAttribute>(),
				"DigitsAttribute should be generated from NHib metadata for Dec property");

			var svad = (DigitsAttribute) sva;

			Assert.That(svad.IntegerDigits, Is.EqualTo(3));
			Assert.That(svad.FractionalDigits, Is.EqualTo(2));
		}

		[Test]
		public void ApplyFromEnumVColumn()
		{
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var vl = ve.GetValidator<FromNhibMetadata>();

			var sva = vl.GetMemberConstraints("EnumV").FirstOrDefault();

			Assert.IsInstanceOf<EnumAttribute>(sva, "EnumAttribute should be generated from NHib metadata for EnumV property");

			var classMapping = cfg.GetClassMapping(typeof(FromNhibMetadata));
			var serialColumn = (Column) classMapping.GetProperty("EnumV").ColumnIterator.Single();
			Assert.That(
				serialColumn.CheckConstraint,
				Is.EqualTo("EnumV in (0, 1)"),
				"Validator annotation should generate valid check for Enums");
		}

		[Test]
		public void ApplyFromComponentStringColumn()
		{
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var vl = ve.GetValidator<FromNhibMetadata>();

			Assert.That(vl.HasValidationRules, Is.True, "Validation rules must be created from NHib metadata");

			var sva = vl.GetMemberConstraints("Cmp").FirstOrDefault();

			Assert.That(
				sva,
				Is.InstanceOf<ValidAttribute>(),
				"ValidAttribute should be generated from NHib metadata for Component property");

			var vli = (IClassValidatorImplementor) vl;
			vl = vli.ChildClassValidators[typeof(Cmp1)];

			sva = vl.GetMemberConstraints("CStrValue").FirstOrDefault();

			Assert.That(sva, Is.Not.Null);
			Assert.That(
				sva,
				Is.InstanceOf<LengthAttribute>(),
				"LengthAttribute should be generated from NHib metadata for Cmp1.CStrValue property");
			var sval = (LengthAttribute) sva;

			Assert.That(sval.Max, Is.EqualTo(3));

			vl = vli.ChildClassValidators[typeof(Cmp2)];

			sva = vl.GetMemberConstraints("CStrValue1").FirstOrDefault();

			Assert.That(
				sva,
				Is.InstanceOf<LengthAttribute>(),
				"LengthAttribute should be generated from NHib metadata for Cmp2.CStrValue1 property");
			sval = (LengthAttribute) sva;

			Assert.That(sval.Max, Is.EqualTo(5));
		}

		[Test]
		public void ApplyConstraintsOnEmbededeComponentsColumns()
		{
			var classMapping = cfg.GetClassMapping(typeof(FromNhibMetadata));
			var col = classMapping.GetProperty("Cmp").ColumnIterator.Cast<Column>().Single(c => c.Name == "CEnumV");

			Assert.That(
				col.CheckConstraint,
				Is.EqualTo("CEnumV in (0, 1)"),
				"Validator annotation should generate valid check for CEnumV column (property of embedded component)");

			var prop = classMapping.GetProperty("Cmps2");
			var col1 = (Mapping.Collection) prop.Value;
			var cmp = (Component) col1.Element;

			col = cmp.ColumnIterator.Cast<Column>().Single(c => c.Name == "CEnumV1");

			Assert.That(
				col.CheckConstraint,
				Is.EqualTo("CEnumV1 in (0, 1)"),
				"Validator annotation should generate valid check for CEnumV1 column (property of embedded component in collection)");
		}

		[Test]
		public void Events()
		{
			var x = new FromNhibMetadata
			{
				Id = 1,
				StrValue = "123456",
				DateNotNull = DateTime.Today,
				Dec = 1234.567M,
				EnumV = (En1) 42
			};

			//x.DateNotNull = null; //!! NHib check not-null itself before integrated validators and throw exception
			// Why it does not check at the same place Length, Precision And scale and so on, if all this known from it configuration?

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				Assert.That(
					() =>
					{
						s.Save(x);
						tx.Commit();
					},
					Throws.InstanceOf<InvalidStateException>()
						  .And.Property(nameof(InvalidStateException.InvalidValues)).Length.EqualTo(3),
					"Saved entity should have raised 3 validation errors");
			}

			x.DateNotNull = null; //But if we will call validators before SavingToNHib Null will be checked
			var ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			var ivals = ve.Validate(x);
			Assert.That(ivals, Has.Length.EqualTo(4), "Unexpected validation error count for first entity with null date");

			// Don't throw exception if it is valid
			x = new FromNhibMetadata
			{
				Id = 2,
				StrValue = "12345",
				DateNotNull = DateTime.Today,
				Dec = 123.45M,
				EnumV = En1.v1
			};

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				Assert.That(
					() =>
					{
						s.Save(x);
						t.Commit();
					},
					Throws.Nothing,
					"Valid entity caused exception on save");
			}

			// Update check
			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				var saved = s.Get<FromNhibMetadata>(2);
				saved.StrValue = "123456";
				//saved.DateNotNull = null;
				saved.DateNotNull = DateTime.Now;
				saved.Dec = 5678.900M;
				saved.EnumV = (En1) 66;

				Assert.That(
					() => t.Commit(),
					Throws.InstanceOf<InvalidStateException>()
						  .And.Property(nameof(InvalidStateException.InvalidValues)).Length.EqualTo(3),
					"Updated entity should have raised 3 validation errors");
			}

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				var saved = s.Get<FromNhibMetadata>(2);
				saved.StrValue = "123";
				saved.DateNotNull = DateTime.Now;
				saved.Dec = 876.54M;
				saved.EnumV = En1.v2;

				Assert.That(
					() => t.Commit(),
					Throws.Nothing,
					"Valid entity caused exception on update");
			}
		}

		[Test]
		public void EventsComponent()
		{
			var x = new FromNhibMetadata
			{
				Id = 3,
				StrValue = "12345",
				DateNotNull = DateTime.Today,
				Dec = 123.45M,
				EnumV = En1.v1,
				Cmp = new Cmp1
				{
					CEnumV = (En1) 66,
					CStrValue = "1234"
				}
			};

			x.Cmps2.Add(new Cmp2 { CEnumV1 = (En1) 66, CStrValue1 = "12345XXXX" });

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				Assert.That(
					() =>
					{
						s.Save(x);
						tx.Commit();
					},
					Throws.InstanceOf<InvalidStateException>()
					      .And.Property(nameof(InvalidStateException.InvalidValues)).Length.EqualTo(4),
					"Saved entity should have raised 4 validation errors");
			}

			x.Cmps2.Clear();
			x.Cmp.CEnumV = En1.v1;
			x.Cmp.CStrValue = "123";

			x.Cmps2.Add(new Cmp2 { CEnumV1 = En1.v2, CStrValue1 = "12345" });

			using (var s = OpenSession())
			using (var t = s.BeginTransaction())
			{
				Assert.That(
					() =>
					{
						s.Save(x);
						t.Commit();
					},
					Throws.Nothing,
					"Valid entity caused exception on save");
			}
		}
	}
}
