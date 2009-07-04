using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintFactory
{
	/// <summary>
	/// Create Validator and keeps the track of how many validators are created.
	/// </summary>
	public class TestConstraintValidatorFactory : DefaultConstraintValidatorFactory
	{
		private static int instanceCounter;

		public static int ValidatorsCreated
		{
			get { return instanceCounter; }
		}

		public static void ClearCounter()
		{
			instanceCounter = 0;
		}

		public override IValidator GetInstance(System.Type type)
		{
			IValidator validator = base.GetInstance(type);
			instanceCounter++;
			return validator;
		}
	}
}