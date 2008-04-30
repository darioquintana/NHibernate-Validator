using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Event
{
	public class NHibernateSharedEngineProvider : ISharedEngineProvider
	{
		private static readonly ValidatorEngine ve = new ValidatorEngine();

		// Explicit static constructor to tell C# compiler not to mark type as before field init
		static NHibernateSharedEngineProvider()
		{
		}

		#region ISharedEngineProvider Members

		public ValidatorEngine GetEngine()
		{
			return ve;
		}

		#endregion
	}
}
