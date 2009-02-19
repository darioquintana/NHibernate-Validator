namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Define how to investigate validators of a class.
	/// </summary>
	public enum ValidatorMode
	{
		/// <summary>
		/// Validators are discovered, by reflection, only investigating the class attributes
		/// </summary>
		UseAttribute,

		/// <summary>
		/// Validator are discovered only parsing the xml of the class
		/// </summary>
		UseExternal, 

		/// <summary>
		/// Validators are discovered, by reflection, investigating the class attributes
		/// and then using the xml(or any other external source), merging both.
		/// Conflicts are solved using xml-configuration over attribute-configuration.
		/// </summary>
		OverrideAttributeWithExternal,

		/// <summary>
		/// Validators are discovered, by reflection, investigating the class attributes
		/// and then using the xml(or any other external source), merging both.
		/// Conflicts are solved using attribute-configuration over xml-configuration.
		/// </summary>
		OverrideExternalWithAttribute
	}
}