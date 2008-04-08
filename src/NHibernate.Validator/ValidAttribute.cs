using System;

namespace NHibernate.Validator
{
    /// <summary>
	/// Enables recursive validation of an associated object
	/// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValidAttribute : Attribute 
    {
    }
}