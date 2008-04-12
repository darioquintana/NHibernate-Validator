namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Hibernate Validator Event properties
	/// The properties are retrieved from Hibernate
	/// (hibernate.properties, hibernate.cfg.xml, persistence.xml or Configuration API)
	/// </summary>
	public class Environment
	{
		/// <summary>
		/// Apply DDL changes on Hibernate metamodel when using validator with Hibernate Annotations. Default to true.
		/// </summary>
		public static readonly string ApplyToDDL = "apply_to_ddl";
		
		/// <summary>
		/// Enable listeners auto registration in Hibernate Annotations and EntityManager. Default to true.
		/// </summary>
		public static readonly string AutoregisterListeners = "autoregister_listeners";

		/// <summary>
		/// Message interpolator class used. The same instance is shared across all ClassValidators 
		/// </summary>
		public static readonly string MessageInterpolatorClass = "message_interpolator_class";

		/// <summary>
		/// Define validation mode.
		/// </summary>
		/// <remarks>
		/// Allowed values are available in <see cref="Engine.ValidatorMode"/>.
		/// </remarks>
		/// <seealso cref="Engine.ValidatorMode"/>
		public static readonly string ValidatorMode = "default-validator-mode";
	}
}