namespace NHibernate.Validator.Cfg.Loquacious
{
	public interface INhIntegration
	{
		INhIntegration And { get; }
		INhIntegration ApplyingDDLConstraints();
		INhIntegration AvoidingDDLConstraints();
		void RegisteringListeners();
		void AvoidingListenersRegister();
	}
}