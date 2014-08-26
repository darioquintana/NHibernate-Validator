using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Base class for all embedded validators, accepting Tags.
	/// </summary>
	[Serializable]
	public abstract class EmbeddedRuleArgsAttribute: ValidationAttribute, ITagableRule, IRuleArgs, IValidator
	{
		private readonly HashSet<object> tagCollection;

		public EmbeddedRuleArgsAttribute()
		{
			tagCollection = new HashSet<object>();
		}

		/// <summary>
		/// Collection of tags.
		/// </summary>
		/// <remarks>
		/// This property should be used only by Attribute.
		/// It is defined as object because language limitation 
		/// (see the end of http://msdn.microsoft.com/en-us/library/aa664616(VS.71).aspx).
		/// </remarks>
		/// <example>
		/// <code>
		/// [Max(100, Tags = new[] { typeof(Error), typeof(Warning) })]
		/// public int Value { get; set; }
		/// </code>
		/// </example>
		public object Tags
		{
			get
			{
				return tagCollection;
			}
			set
			{
				if(value == null)
				{
					return;
				}
				if(value.GetType().IsArray)
				{
					var a = (IEnumerable) value;
					foreach (var t in a)
					{
						tagCollection.Add(t);
					}
				}
				else
				{
					tagCollection.Clear();
					tagCollection.Add(value);
				}
			}
		}

		ICollection<object> ITagableRule.TagCollection
		{
			get { return tagCollection; }
		}

		public string Message
		{
			get
			{
				return (this.ErrorMessage);
			}
			set
			{
				this.ErrorMessage = value;
			}
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var result = ValidationResult.Success;
			var context = new ConstraintValidatorContext(string.Empty, this.Message);

			if (this.IsValid(value, context) == false)
			{
				result = new ValidationResult(this.Message, new string[] { validationContext.MemberName });
			}

			return result;
		}

		public override bool IsValid(object value)
		{
			return this.IsValid(value, new ConstraintValidatorContext(string.Empty, this.Message));
		}

		public abstract bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext);
	}
}