using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.ConstraintContext.EntityValidation;
using NHibernate.Validator.Tests.ConstraintContext.MemberValidation;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ConstraintContext
{
	[TestFixture]
	public class ConstraintContextIntegrationFixture
	{
		/// <summary>
		/// Member validation
		/// </summary>
		[Test]
		public void ShouldAddAnothersMessages()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo
			         	{
			         		Username = null,
			         		Password = "123X"
			         	};

			InvalidValue[] invalidValues = vtor.Validate(mi);
			Assert.AreEqual(3, invalidValues.Length);
		}

		/// <summary>
		/// Member validation
		/// </summary>
		[Test]
		public void ShouldAddAnothersMessagesUsingValidationProperties()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo
			{
				Username = null,
				Password = "123X"
			};

			InvalidValue[] invalidValues = vtor.ValidatePropertyValue(mi, x => x.Password);
			Assert.AreEqual(2, invalidValues.Length);
			Assert.AreEqual(Messages.PasswordLength, invalidValues.ElementAt(0).Message);
			Assert.AreEqual(Messages.PasswordContent, invalidValues.ElementAt(1).Message);
		}

		/// <summary>
		/// Member validation
		/// </summary>
		[Test]
		public void ShouldAddAnothersMessagesUsingEntityValidation()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo2
			         	{
			         		Username = null,
			         		Password = "123X"
			         	};

			InvalidValue[] invalidValues = vtor.Validate(mi);
			Assert.AreEqual(3, invalidValues.Length);
			
			Assert.AreEqual(Messages.PasswordLength, invalidValues.ElementAt(1).Message);
			Assert.AreEqual(Messages.PasswordContent, invalidValues.ElementAt(2).Message);
		}

		/// <summary>
		/// Entity validation
		/// </summary>
		[Test]
		public void ShouldDisableTheDefaultMessageAndAddAnothers()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo
			         	{
			         		Username = "MyName",
							Password = "123X"
			         	};

			InvalidValue[] invalidValues = vtor.Validate(mi);

			Assert.AreEqual(2, invalidValues.Length);
			Assert.AreEqual(Messages.PasswordLength, invalidValues.ElementAt(0).Message);
			Assert.AreEqual(Messages.PasswordContent, invalidValues.ElementAt(1).Message);
		}

		/// <summary>
		/// Entity validation
		/// </summary>
		[Test]
		public void ShouldDisableTheDefaultMessageAndAddAnothersUsingEntityValidation()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo2
			         	{
			         		Username = "MyName",
			         		Password = "123XMyName"
			         	};

			InvalidValue[] invalidValues = vtor.Validate(mi);

			Assert.AreEqual(2, invalidValues.Length);
			Assert.AreEqual(Messages.PasswordContentUsername, invalidValues.ElementAt(0).Message);
			Assert.AreEqual(Messages.PasswordContent, invalidValues.ElementAt(1).Message);
		}
	}
}