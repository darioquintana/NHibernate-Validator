using System.Collections.Generic;
using log4net.Config;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV92
{
	public class FluentValidationWithAttributesBugTest
	{
		public class SubEntity
		{
			private string _subStringValue;
			private bool _boolValue;

			[NotNullNotEmpty(Message = "Attribute validation")]
			public string SubStringValue
			{
				get { return _subStringValue; }
				set { _subStringValue = value; }
			}

			public bool BoolValue
			{
				get { return _boolValue; }
				set { _boolValue = value; }
			}
		}

		public class Entity
		{
			private string _stringValue;
			private List<SubEntity> _subEntities;

			[NotNullNotEmpty(Message = "Attribute validation")]
			public string StringValue
			{
				get { return _stringValue; }
				set { _stringValue = value; }
			}

			[Valid]
			public List<SubEntity> SubEntities
			{
				get { return _subEntities; }
				set { _subEntities = value; }
			}
		}

		public class SubEntityValidation : ValidationDef<SubEntity>
		{
			public SubEntityValidation()
			{
				Define(x => x.BoolValue).IsTrue().WithMessage("Fluent validation");
			}
		}

		private Entity _entity;

		[SetUp]
		public void SetUp()
		{
			XmlConfigurator.Configure();
			_entity = new Entity
			{
				SubEntities = new List<SubEntity>{
                    new SubEntity()
                }
			};
		}

		[Test]
		public void Validating_root_entity_should_report_error_from_attribute_validators_and_fluent_validators()
		{
			var engine = new ValidatorEngine();
			var cfg = new FluentConfiguration();
			cfg.Register(new[] { typeof(SubEntityValidation) });
			cfg.SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute);
			engine.Configure(cfg);

			InvalidValue[] errors = engine.Validate(_entity);

			Assert.That(errors.Length, Is.EqualTo(3)); // FAIL! validators defined in SubEntityValidation are not considered
		}

		public class EntityValidation : ValidationDef<Entity>
		{
			public EntityValidation()
			{
				Define(x => x.SubEntities).HasValidElements();
			}
		}

		[Test]
		public void Workaround()
		{
			var engine = new ValidatorEngine();
			var cfg = new FluentConfiguration();
			cfg.Register(new[]{
                typeof(SubEntityValidation),
                typeof(EntityValidation) // Here is the workaround
            });
			cfg.SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute);
			engine.Configure(cfg);

			InvalidValue[] errors = engine.Validate(_entity);

			Assert.That(errors.Length, Is.EqualTo(3));
		}
	}
}