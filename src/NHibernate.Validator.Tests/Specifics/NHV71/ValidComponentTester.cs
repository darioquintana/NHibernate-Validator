using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV71
{
	[TestFixture]
	public class ValidComponentTester : PersistenceTest
	{
		protected override IList Mappings
		{
			get { return new[] { "Specifics.NHV71.Customer.hbm.xml" }; }
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			base.Configure(configuration);
			var nhvc = new FluentConfiguration();
			nhvc.SetDefaultValidatorMode(ValidatorMode.UseAttribute).IntegrateWithNHibernate.ApplyingDDLConstraints().And.
				RegisteringListeners();
			var onlyToUseToInitializeNh_Engine = new ValidatorEngine();
			onlyToUseToInitializeNh_Engine.Configure(nhvc);
			configuration.Initialize(onlyToUseToInitializeNh_Engine);
		}
		private Customer GetNotValidCustomer()
		{
			return new Customer { Name = new string('*', 11), Contact = new ContactInfo { Email = "bad_mail" } };
		}

		[Test]
		public void when_validate_customer_with_invalid_name_and_email_should_return_two_invalid_values()
		{
			var validatorEngine = new ValidatorEngine();
			var notValidCustomer = GetNotValidCustomer();
			validatorEngine.Configure();

			validatorEngine.Validate(notValidCustomer).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void when_commit_customer_with_invalid_name_and_email_should_throw_invalid_state_ex_with_two_invalid_values()
		{
			var notValidCustomer = GetNotValidCustomer();
			using (var session = OpenSession())
			{
				try
				{
					using (var tx = session.BeginTransaction())
					{
						session.Save(notValidCustomer);
						tx.Commit();
					}
					Assert.False(true, "Commit should throw InvalidStateException.");
				}
				catch (InvalidStateException ex)
				{
					var invalidValues = ex.GetInvalidValues();
					invalidValues.Should().Have.Count.EqualTo(2);
				}
			}
		}

	}
}