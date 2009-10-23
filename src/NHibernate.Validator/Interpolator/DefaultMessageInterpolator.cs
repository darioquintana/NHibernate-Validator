using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Interpolator
{
	[Serializable]
	public class DefaultMessageInterpolator : IMessageInterpolator, ISerializable
	{
		private readonly Dictionary<string, object> attributeParameters = new Dictionary<string, object>();
		private string attributeMessage;

		[NonSerialized] private CultureInfo culture = CultureInfo.CurrentUICulture;
		[NonSerialized] private ResourceManager defaultMessageBundle;
		[NonSerialized] private string interpolateMessage;
		[NonSerialized] private ResourceManager messageBundle;

		public string AttributeMessage
		{
			get { return attributeMessage; }
		}

		public DefaultMessageInterpolator()
		{
		}

		public DefaultMessageInterpolator (SerializationInfo info, StreamingContext context)
		{
			attributeParameters = (Dictionary<string, object>) info.GetValue("attributeParameters",typeof(Dictionary<string, object>) );
			attributeMessage = (string) info.GetValue("message",typeof(string));
		}

		#region IMessageInterpolator Members

		public virtual string Interpolate(string message, object entity, IValidator validator, IMessageInterpolator defaultInterpolator)
		{
			bool same = attributeMessage.Equals(message);
            if (same && interpolateMessage != null && !message.Contains("${"))
			{
				return interpolateMessage; //short cut
			}

			string result = Replace(message,entity);
			if (same)
			{
				interpolateMessage = result; //short cut in next iteration
			}
			return result;
		}

		#endregion

		public void Initialize(ResourceManager messageBundle, ResourceManager defaultMessageBundle, CultureInfo culture)
		{
			this.culture = culture ?? CultureInfo.CurrentCulture;
			this.messageBundle = messageBundle;
			this.defaultMessageBundle = defaultMessageBundle;
		}

		public void Initialize(Attribute attribute)
		{
			//Get all parametters of the Attribute: the name and their values.
			//For example:
			//In LengthAttribute the parametter are Min and Max.
			System.Type clazz = attribute.GetType();

			IRuleArgs ruleArgs = attribute as IRuleArgs;

			if (ruleArgs == null)
			{
				throw new ArgumentException("Attribute " + clazz + " doesn't implement IRuleArgs interface.");
			}
			else
			{
				if (ruleArgs.Message == null)
				{
					throw new ArgumentException(string.Format("The value of the message in {0} attribute is null (nothing for VB.Net Users). Add some message ", clazz) +
"on the attribute to solve this issue. For Example:\n" +
"- private string message = \"Error on property Foo\";\n" +
"Or you can use interpolators with resources files:\n" +
"-private string message = \"{validator.MyValidatorMessage}\";");
				}
				
				attributeMessage = ruleArgs.Message;
			}

			foreach (PropertyInfo property in clazz.GetProperties())
			{
				attributeParameters.Add(property.Name.ToLowerInvariant(), property.GetValue(attribute, null));
			}
		}

		protected virtual string Replace(string message, object entity)
		{
			var translator = new MessageTranslator(message);
			var revelevantTokens = translator.RelevantTokens;
			return translator.Replace(ExtractReplacements(revelevantTokens, entity));
		}

		protected virtual IDictionary<string, string> ExtractReplacements(IEnumerable<string> revelevantTokens, object entity)
		{
			var replacements = new Dictionary<string, string>(20);
			foreach (var revelevantToken in revelevantTokens)
			{
				if (revelevantToken.StartsWith("$"))
				{
					var value = GetPropertyValue(entity, revelevantToken.Trim('$', '{', '}'));
					replacements[revelevantToken] = value == null ? null : value.ToString();
				}
				else
				{
					var replacement = GetAttributeOrResourceValue(revelevantToken.Trim('{', '}'));
					if (replacement != null)
					{
						// the message in the resource may need to be parsed
						replacement = Replace(replacement, entity);
					}
					if (replacement != null)
					{
						// the token need to be replaced; otherwise we leave the token itself
						replacements[revelevantToken] = replacement;
					}
				}
			}
			return replacements;
		}

		/// <summary>
		/// Get the value of an Attribute's property or the value in the resource for a given key.
		/// </summary>
		/// <param name="token">The property-name of the attribute or key to find in the resources.</param>
		/// <returns>The string in the resource or null where not found.</returns>
		protected virtual string GetAttributeOrResourceValue(string token)
		{
			object attributeValue;
			if (attributeParameters.TryGetValue(token.ToLowerInvariant(), out attributeValue))
			{
				return attributeValue == null ? null : attributeValue.ToString();
			}

			string resourceValue = null;
			try
			{
				resourceValue = messageBundle != null ? messageBundle.GetString(token, culture) : null;
			}
			catch (MissingManifestResourceException e)
			{
				//give a second chance with the default resource bundle
			}
			if (resourceValue == null)
			{
				resourceValue = defaultMessageBundle.GetString(token, culture);
				// in this case we don't catch the MissingManifestResourceException because
				// we are sure that we DefaultValidatorMessages.resx is an embedded resource
			}
			return resourceValue;
		}

		/// <summary>
		/// Override this method to obtain flexibility.
		/// The default interpolator can replace the message with public property values.
		/// </summary>
		/// <param name="entity">Entity or value</param>
		/// <param name="propertyName">Property name to be used.</param>
		/// <returns>The value of the property</returns>
		protected virtual object GetPropertyValue(object entity, string propertyName)
		{
			if (!propertyName.Contains("."))
			{
				var property = entity.GetType().GetProperty(propertyName);
				if (property == null) throw new InvalidPropertyNameException(propertyName, entity.GetType());

				return property.GetValue(entity, null);
			}
			else
			{
				var membersChain = propertyName.Split('.');
				object value = entity;
				foreach (var memberName in membersChain)
				{
					var property = value.GetType().GetProperty(memberName);
					if (property == null) throw new InvalidPropertyNameException(memberName, entity.GetType());
					value = property.GetValue(value, null);
				}
				return value;
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("attributeParameters",attributeParameters );
			info.AddValue("message",attributeMessage );
		}
	}
}