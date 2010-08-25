using System.Linq;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV93
{
	[TestFixture]
	public class FullPathTest
	{
		public class Entity
		{
			public Entity SubEntity { get; set; }
			public bool Valid { get; set; }
		}

		public class EntityValidation : ValidationDef<Entity>
		{
			public EntityValidation()
			{
				Define(x => x.SubEntity).IsValid();
				ValidateInstance.By((x, c) => x.Valid);
			}
		}

		[Test]
		public void Should_format_propertypath_correctly()
		{
			var engine = new ValidatorEngine();
			var cfg = new FluentConfiguration();
			cfg.Register(new[] {typeof (EntityValidation)});
			cfg.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			engine.Configure(cfg);
			var entity = new Entity
			             	{
			             		Valid = true,
			             		SubEntity = new Entity
			             		            	{
			             		            		Valid = false
			             		            	}
			             	};
			InvalidValue[] errors = engine.Validate(entity);
			Assert.That(errors.Single().PropertyPath, Is.EqualTo("SubEntity"));
		}
	}
}