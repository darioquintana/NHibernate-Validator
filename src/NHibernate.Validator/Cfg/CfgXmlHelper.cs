namespace NHibernate.Validator.Cfg
{
	/// <summary>
	/// Helper to parse validator-configuration XmlNode.
	/// </summary>
	public static class CfgXmlHelper
	{
		public static ValidatorMode ValidatorModeConvertFrom(string validatorMode)
		{
			if (string.IsNullOrEmpty(validatorMode))
				return ValidatorMode.UseAttribute;

			string vm = validatorMode.ToLowerInvariant();
			switch (vm)
			{
				case "useattribute":
					return ValidatorMode.UseAttribute;

				case "usexml":
					return ValidatorMode.UseXml;

				case "ovverrideattributewithxml":
					return ValidatorMode.OverrideAttributeWithXml;

				case "overridexmlwithattribute":
					return ValidatorMode.OverrideXmlWithAttribute;

				default:
					throw new ValidatorConfigurationException("Unexpected ValidatorMode :" + validatorMode);
			}
		}

		public static bool ModeAcceptsAttributes(ValidatorMode validatorMode)
		{
			return validatorMode != ValidatorMode.UseXml;
		}

		public static bool ModeAcceptsXml(ValidatorMode validatorMode)
		{
			return validatorMode != ValidatorMode.UseAttribute;
		}
	}
}
