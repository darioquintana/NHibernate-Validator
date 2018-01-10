using System;
using NHibernate.Validator.Cfg.Loquacious.Impl;

namespace NHibernate.Validator.Cfg.Loquacious
{
	// 6.0 TODO: move to INhIntegration.
	public static class NhIntegrationExtension
	{
		public static INhIntegration ApplyingGenerationFromMapping(this INhIntegration integration)
		{
			var impl = GetImplementation(integration);
			return impl.ApplyingGenerationFromMapping();
		}

		public static INhIntegration AvoidingGenerationFromMapping(this INhIntegration integration)
		{
			var impl = GetImplementation(integration);
			return impl.AvoidingGenerationFromMapping();
		}

		private static NhIntegration GetImplementation(INhIntegration integration)
		{
			return integration as NhIntegration ??
				throw new NotSupportedException(
					"Only NHibernate.Validator.Cfg.Loquacious.Impl.NhIntegration implementations are supported by NhIntegrationExtension");
		}
	}

	public interface INhIntegration
	{
		INhIntegration And { get; }

		INhIntegration ApplyingDDLConstraints();
		INhIntegration AvoidingDDLConstraints();

		void RegisteringListeners();
		void AvoidingListenersRegister();
	}
}
