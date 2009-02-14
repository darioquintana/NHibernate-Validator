namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class NhIntegration: INhIntegration
	{
		private readonly INHVConfiguration configuration;

		public NhIntegration(INHVConfiguration configuration)
		{
			this.configuration = configuration;
		}

		#region Implementation of INhIntegration

		public INhIntegration And
		{
			get { return this; }
		}

		public INhIntegration ApplyingDDLConstraints()
		{
			configuration.Properties[Environment.ApplyToDDL] = "true";
			return this;
		}

		public INhIntegration AvoidingDDLConstraints()
		{
			configuration.Properties[Environment.ApplyToDDL] = "false";
			return this;
		}

		public void RegisteringListeners()
		{
			configuration.Properties[Environment.AutoregisterListeners] = "true";
		}

		public void AvoidingListenersRegister()
		{
			configuration.Properties[Environment.AutoregisterListeners] = "false";
		}

		#endregion
	}
}