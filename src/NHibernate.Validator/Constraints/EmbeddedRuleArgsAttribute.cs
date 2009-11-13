using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Base class for all embedded validators, accepting Tags.
	/// </summary>
	[Serializable]
	public class EmbeddedRuleArgsAttribute: Attribute, ITagableRule
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
	}
}