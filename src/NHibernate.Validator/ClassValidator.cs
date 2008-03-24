using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using Iesi.Collections;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Util;
using NHibernate.Validator.Interpolator;
using NHibernate.Validator.MappingSchema;
using NHibernate.Validator.XmlConfiguration;

namespace NHibernate.Validator
{
	/// <summary>
	/// Engine that take a object and check every expressed attribute restrictions
	/// </summary>
	[Serializable]
	public class ClassValidator : IClassValidator
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ClassValidator));

		private readonly BindingFlags AnyVisibilityInstanceAndStatic = (BindingFlags.NonPublic | BindingFlags.Public
																																		| BindingFlags.Instance | BindingFlags.Static);

		private readonly System.Type beanClass;

		private DefaultMessageInterpolatorAggregator defaultInterpolator;

		[NonSerialized]
		private readonly ResourceManager messageBundle;

		[NonSerialized]
		private readonly ResourceManager defaultMessageBundle;

		[NonSerialized]
		private readonly IMessageInterpolator userInterpolator;

		private readonly Dictionary<System.Type, ClassValidator> childClassValidators;

		private IList<IValidator> beanValidators;

		private IList<IValidator> memberValidators;

		private List<MemberInfo> memberGetters;

		private List<MemberInfo> childGetters;

		private static readonly InvalidValue[] EMPTY_INVALID_VALUE_ARRAY = new InvalidValue[] { };

		private readonly CultureInfo culture;

		private readonly ValidatorMode validatorMode;


		/// <summary>
		/// Create the validator engine for this bean type
		/// </summary>
		/// <param name="beanClass"></param>
		public ClassValidator(System.Type beanClass)
			: this(beanClass, null, null, ValidatorMode.UseAttribute) { }

		/// <summary>
		/// Create the validator engine for this bean type
		/// </summary>
		/// <param name="beanClass"></param>
		/// <param name="validatorMode">Validator definition mode</param>
		public ClassValidator(System.Type beanClass, ValidatorMode validatorMode)
			: this(beanClass, null, null, validatorMode) { }

		/// <summary>
		/// Create the validator engine for a particular bean class, using a resource bundle
		/// for message rendering on violation
		/// </summary>
		/// <param name="beanClass">bean type</param>
		/// <param name="resourceManager"></param>
		/// <param name="culture">The CultureInfo for the <paramref name="beanClass"/>.</param>
		/// <param name="validatorMode">Validator definition mode</param>
		public ClassValidator(System.Type beanClass, ResourceManager resourceManager, CultureInfo culture, ValidatorMode validatorMode)
			: this(beanClass, resourceManager, culture, null, new Dictionary<System.Type, ClassValidator>(), validatorMode)
		{
		}

		/// <summary>
		/// Create the validator engine for a particular bean class, using a custom message interpolator
		/// for message rendering on violation
		/// </summary>
		/// <param name="beanClass"></param>
		/// <param name="interpolator"></param>
		public ClassValidator(System.Type beanClass, IMessageInterpolator interpolator)
			: this(beanClass, null, null, interpolator, new Dictionary<System.Type, ClassValidator>(), ValidatorMode.UseAttribute)
		{
		}

		/// <summary>
		/// Not a public API
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="resourceManager"></param>
		/// <param name="culture"></param>
		/// <param name="userInterpolator"></param>
		/// <param name="childClassValidators"></param>
		/// <param name="validatorMode">Validator definition mode.</param>
		internal ClassValidator(
			System.Type clazz,
			ResourceManager resourceManager,
			CultureInfo culture,
			IMessageInterpolator userInterpolator,
			Dictionary<System.Type, ClassValidator> childClassValidators, 
			ValidatorMode validatorMode)
		{
			beanClass = clazz;

			messageBundle = resourceManager ?? GetDefaultResourceManager();
			defaultMessageBundle = GetDefaultResourceManager();
			this.culture = culture;
			this.userInterpolator = userInterpolator;
			this.childClassValidators = childClassValidators;
			this.validatorMode = validatorMode;

			//Initialize the ClassValidator
			InitValidator(beanClass, childClassValidators);
		}

		public ClassValidator(System.Type type, CultureInfo culture)
			: this(type)
		{
			this.culture = culture;
		}

		/// <summary>
		/// Return true if this <see cref="ClassValidator"/> contains rules for apply, false in other case. 
		/// </summary>
		public bool HasValidationRules
		{
			get
			{
				return beanValidators.Count != 0 || memberValidators.Count != 0;
			}
		}

		private static ResourceManager GetDefaultResourceManager()
		{
			return new ResourceManager("NHibernate.Validator.Resources.DefaultValidatorMessages",
																 Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// Initialize the <see cref="ClassValidator"/> type.
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="childClassValidators"></param>
		private void InitValidator(System.Type clazz, IDictionary<System.Type, ClassValidator> childClassValidators)
		{
			beanValidators = new List<IValidator>();
			memberValidators = new List<IValidator>();
			memberGetters = new List<MemberInfo>();
			childGetters = new List<MemberInfo>();
			defaultInterpolator = new DefaultMessageInterpolatorAggregator();
			defaultInterpolator.Initialize(messageBundle, defaultMessageBundle, culture);

			//build the class hierarchy to look for members in
			childClassValidators.Add(clazz, this);
			ISet<System.Type> classes = new HashedSet<System.Type>();
			AddSuperClassesAndInterfaces(clazz, classes);

			foreach (System.Type currentClass in classes)
			{
				foreach (Attribute classAttribute in currentClass.GetCustomAttributes(false))
				{
					IValidator validator = CreateValidator(classAttribute);

					if (validator != null)
					{
						beanValidators.Add(validator);
					}

					//Note: No need to handle Aggregate annotations, c# use Multiple Attribute declaration.
					//HandleAggregateAnnotations(classAttribute, null);
				}
			}

			//Check on all selected classes
			foreach (System.Type currentClass in classes)
			{
				if (validatorMode == ValidatorMode.UseXml)
				{
					CreateMembersFromXml(currentClass);
				}
				else
				{
					foreach (PropertyInfo currentProperty in currentClass.GetProperties())
					{
						CreateMemberValidator(currentProperty);
						CreateChildValidator(currentProperty);
					}

					foreach (FieldInfo currentField in currentClass.GetFields(AnyVisibilityInstanceAndStatic))
					{
						CreateMemberValidator(currentField);
						CreateChildValidator(currentField);
					}
				}
			}
		}

		// TODO: AddDirectoryInfo(DirectoryInfo directory), AddFile(string xmlFile) AddXmlFile(string XmlFile)

		private static List<string> GetAllNHVXmlResourceNames(Assembly assembly)
		{
			List<string> result = new List<string>();

			foreach (string resource in assembly.GetManifestResourceNames())
			{
				if (resource.EndsWith(".nhv.xml"))
				{
					log.Info(resource);
					result.Add(resource);
				}
			}

			return result;
		}

		private void CreateMembersFromXml(System.Type currentClass)
		{
			IMappingDocumentParser parser = new MappingDocumentParser();
			Stream stream = Assembly.GetAssembly(currentClass).GetManifestResourceStream(currentClass.FullName + ".nhv.xml");

			if (stream != null)
			{
				NhvValidator validator = parser.Parse(stream);

				foreach (NhvProperty property in validator.property)
				{
					MemberInfo currentMember = GetPropertyOrField(currentClass, property.name);

                    if (currentMember == null)
                    {
                        log.Error("Property or field name was not found in the class");
                        continue;
                    }
                    log.Info("Looking for rules for property : {0}" + property.name);
                    CreateMemberValidatorFromRules(currentMember, property.rules);
                    CreateChildValidator(currentMember);
				}
			}
		}

        private MemberInfo GetPropertyOrField(System.Type currentClass, string name)
        {
            MemberInfo memberInfo = currentClass.GetProperty(name);
            if (memberInfo == null)
            {
                memberInfo = currentClass.GetField(name);
            }

            return memberInfo;
        }

		private void CreateMemberValidatorFromRules(MemberInfo member, NhvRules rules)
		{
			foreach (object rule in rules.Items)
			{
				IValidator propertyValidator = CreateValidatorFromRule(rule);

				if (propertyValidator != null)
				{
					memberValidators.Add(propertyValidator);
					memberGetters.Add(member);
				}
			}
		}

		private IValidator CreateValidatorFromRule(object rule)
		{
			Attribute thisAttribute = XmlRulesFactory.CreateAttributeFromRule(rule);

			if (thisAttribute != null)
			{
				return CreateValidator(thisAttribute);
			}

			log.Info("No rule found");
			return null;
		}

		/// <summary>
		/// apply constraints on a bean instance and return all the failures.
		/// if <see cref="bean"/> is null, an empty array is returned 
		/// </summary>
		/// <param name="bean">object to apply the constraints</param>
		/// <returns></returns>
		public InvalidValue[] GetInvalidValues(object bean)
		{
			return GetInvalidValues(bean, new IdentitySet());
		}

		/// <summary>
		/// Not a public API
		/// </summary>
		/// <param name="bean"></param>
		/// <param name="circularityState"></param>
		/// <returns></returns>
		private InvalidValue[] GetInvalidValues(object bean, ISet circularityState)
		{
			if (bean == null || circularityState.Contains(bean))
			{
				return EMPTY_INVALID_VALUE_ARRAY; //Avoid circularity
			}
			else
			{
				circularityState.Add(bean);
			}

			if (!beanClass.IsInstanceOfType(bean))
			{
				throw new ArgumentException("not an instance of: " + bean.GetType());
			}

			List<InvalidValue> results = new List<InvalidValue>();

			//Bean Validation
			foreach (IValidator validator in beanValidators)
			{
				if (!validator.IsValid(bean))
				{
					results.Add(new InvalidValue(Interpolate(validator), beanClass, null, bean, bean));
				}
			}

			//Property & Field Validation
			for (int i = 0; i < memberValidators.Count; i++)
			{
				MemberInfo member = memberGetters[i];

				if (NHibernateUtil.IsPropertyInitialized(bean, member.Name))
				{
					object value = GetMemberValue(bean, member);

					IValidator validator = memberValidators[i];

					if (!validator.IsValid(value))
					{
						results.Add(new InvalidValue(Interpolate(validator), beanClass, member.Name, value, bean));
					}
				}
			}

			//Child validation
			for (int i = 0; i < childGetters.Count; i++)
			{
				MemberInfo member = childGetters[i];

				if (NHibernateUtil.IsPropertyInitialized(bean, member.Name))
				{
					object value = GetMemberValue(bean, member);

					if (value != null && NHibernateUtil.IsInitialized(value))
					{
						MakeChildValidation(value, bean, member, circularityState, results);
					}
				}
			}
			return results.ToArray();
		}

		/// <summary>
		/// Validate the child validation to objects and collections
		/// </summary>
		/// <param name="value">value to validate</param>
		/// <param name="bean"></param>
		/// <param name="member"></param>
		/// <param name="circularityState"></param>
		/// <param name="results"></param>
		private void MakeChildValidation(object value, object bean, MemberInfo member, ISet circularityState, IList<InvalidValue> results)
		{
			IEnumerable valueEnum = value as IEnumerable;
			if (valueEnum != null)
			{
				MakeChildValidation(valueEnum, bean, member, circularityState, results);
			}
			else
			{
				//Simple Value, Not a Collection
				InvalidValue[] invalidValues = GetClassValidator(value)
					.GetInvalidValues(value, circularityState);

				foreach (InvalidValue invalidValue in invalidValues)
				{
					invalidValue.AddParentBean(bean, member.Name);
					results.Add(invalidValue);
				}
			}
		}

		/// <summary>
		/// Validate the child validation to collections
		/// </summary>
		/// <param name="value"></param>
		/// <param name="bean"></param>
		/// <param name="member"></param>
		/// <param name="circularityState"></param>
		/// <param name="results"></param>
		private void MakeChildValidation(IEnumerable value, object bean, MemberInfo member, ISet circularityState, IList<InvalidValue> results)
		{
			if (IsGenericDictionary(value.GetType())) //Generic Dictionary
			{
				int index = 0;
				foreach (object item in value)
				{
					if (item == null)
					{
						index++;
						continue;
					}

					IGetter ValueProperty = new BasicPropertyAccessor().GetGetter(item.GetType(), "Value");
					IGetter KeyProperty = new BasicPropertyAccessor().GetGetter(item.GetType(), "Key");

					InvalidValue[] invalidValuesKey = GetClassValidator(ValueProperty.Get(item)).GetInvalidValues(ValueProperty.Get(item), circularityState);
					String indexedPropName = string.Format("{0}[{1}]", member.Name, index);

					foreach (InvalidValue invalidValue in invalidValuesKey)
					{
						invalidValue.AddParentBean(bean, indexedPropName);
						results.Add(invalidValue);
					}

					InvalidValue[] invalidValuesValue = GetClassValidator(KeyProperty.Get(item)).GetInvalidValues(KeyProperty.Get(item), circularityState);
					indexedPropName = string.Format("{0}[{1}]", member.Name, index);

					foreach (InvalidValue invalidValue in invalidValuesValue)
					{
						invalidValue.AddParentBean(bean, indexedPropName);
						results.Add(invalidValue);
					}

					index++;
				}
			}
			else //Generic collection
			{
				int index = 0;
				foreach (object item in value)
				{
					if (item == null)
					{
						index++;
						continue;
					}

					InvalidValue[] invalidValues = GetClassValidator(item).GetInvalidValues(item, circularityState);

					String indexedPropName = string.Format("{0}[{1}]", member.Name, index);

					index++;

					foreach (InvalidValue invalidValue in invalidValues)
					{
						invalidValue.AddParentBean(bean, indexedPropName);
						results.Add(invalidValue);
					}
				}
			}
		}

		/// <summary>
		/// Get the ClassValidator for the <see cref="Type"/> of the <see cref="value"/>
		/// parametter  from <see cref="childClassValidators"/>. If doesn't exist, a 
		/// new <see cref="ClassValidator"/> is returned.
		/// </summary>
		/// <param name="value">object to get type</param>
		/// <returns></returns>
		private ClassValidator GetClassValidator(object value)
		{
			System.Type clazz = value.GetType();

			ClassValidator classValidator = childClassValidators[clazz];

			return classValidator ?? new ClassValidator(clazz);
		}

		/// <summary>
		/// Get the message of the <see cref="IValidator"/> and 
		/// interpolate it.
		/// </summary>
		/// <param name="validator"></param>
		/// <returns></returns>
		private string Interpolate(IValidator validator)
		{
			String message = defaultInterpolator.GetAttributeMessage(validator);

			if (userInterpolator != null)
			{
				return userInterpolator.Interpolate(message, validator, defaultInterpolator);
			}
			else
			{
				return defaultInterpolator.Interpolate(message, validator, null);
			}
		}

		/// <summary>
		/// Create a <see cref="IValidator{A}"/> from a <see cref="ValidatorClassAttribute"/> attribute.
		/// If the attribute is not a <see cref="ValidatorClassAttribute"/> type return null.
		/// </summary>
		/// <param name="attribute">attribute</param>
		/// <returns>the validator for the attribute</returns>
		private IValidator CreateValidator(Attribute attribute)
		{
			try
			{
				ValidatorClassAttribute validatorClass = null;
				object[] AttributesInTheAttribute = attribute.GetType().GetCustomAttributes(typeof(ValidatorClassAttribute), false);

				if (AttributesInTheAttribute.Length > 0)
				{
					validatorClass = (ValidatorClassAttribute)AttributesInTheAttribute[0];
				}

				if (validatorClass == null)
				{
					return null;
				}

				IValidator beanValidator = (IValidator)Activator.CreateInstance(validatorClass.Value);
				beanValidator.Initialize(attribute);
				defaultInterpolator.AddInterpolator(attribute, beanValidator);
				return beanValidator;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("could not instantiate ClassValidator", ex);
			}
		}

		/// <summary>
		/// Create a Validator from a property or field.
		/// </summary>
		/// <param name="member"></param>
		private void CreateMemberValidator(MemberInfo member)
		{
			object[] memberAttributes = member.GetCustomAttributes(false);

			foreach (Attribute memberAttribute in memberAttributes)
			{
				IValidator propertyValidator = CreateValidator(memberAttribute);

				if (propertyValidator != null)
				{
					memberValidators.Add(propertyValidator);
					memberGetters.Add(member);
				}
			}
		}

		/// <summary>
		/// Create the validator for the children, who got the <see cref="ValidAttribute"/>
		/// on the fields or properties
		/// </summary>
		/// <param name="member"></param>
		private void CreateChildValidator(MemberInfo member)
		{
			if (!member.IsDefined(typeof(ValidAttribute), false)) return;

			KeyValuePair<System.Type, System.Type> clazzDictionary;
			System.Type clazz;

			childGetters.Add(member);

			if (IsGenericDictionary(GetType(member)))
			{
				clazzDictionary = GetGenericTypesOfDictionary(member);
				if (!childClassValidators.ContainsKey(clazzDictionary.Key))
					new ClassValidator(clazzDictionary.Key, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);
				if (!childClassValidators.ContainsKey(clazzDictionary.Value))
					new ClassValidator(clazzDictionary.Value, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);

				return;
			}
			else
			{
				clazz = GetTypeOfMember(member);
			}

			if (!childClassValidators.ContainsKey(clazz))
			{
				new ClassValidator(clazz, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);
			}
		}

		/// <summary>
		/// Get the Generic Arguments of a <see cref="IDictionary{TKey,TValue}"/>
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		private static KeyValuePair<System.Type, System.Type> GetGenericTypesOfDictionary(MemberInfo member)
		{
			System.Type clazz = GetType(member);

			return new KeyValuePair<System.Type, System.Type>(clazz.GetGenericArguments()[0], clazz.GetGenericArguments()[1]);
		}

		/// <summary>
		/// Get the type of the a Field or Property. 
		/// If is a: Generic Collection or a Array, return the type of the elements.
		/// TODO: Refactor this method to some Utils.
		/// </summary>
		/// <param name="member">MemberInfo, represent a property or field</param>
		/// <returns>type of the member or collection member</returns>
		private static System.Type GetTypeOfMember(MemberInfo member)
		{
			System.Type clazz = GetType(member);

			if (clazz.IsArray) // Is Array
			{
				return clazz.GetElementType();
			}
			else if (IsEnumerable(clazz) && clazz.IsGenericType) //Is Collection Generic  
			{
				return clazz.GetGenericArguments()[0];
			}

			return clazz; //Single type, not a collection/array
		}

		/// <summary>
		/// Indicates if a <see cref="Type"/> is <see cref="IEnumerable"/>
		/// </summary>
		/// <param name="clazz"></param>
		/// <returns>is enumerable or not</returns>
		private static bool IsEnumerable(System.Type clazz)
		{
			return clazz.GetInterface(typeof(IEnumerable).FullName) == null ? false : true;
		}

		private static bool IsGenericDictionary(System.Type clazz)
		{
			if (clazz.IsInterface && clazz.IsGenericType)
				return typeof(IDictionary<,>).Equals(clazz.GetGenericTypeDefinition());
			else
				return clazz.GetInterface(typeof(IDictionary<,>).Name) == null ? false : true;
		}

		/// <summary>
		/// Get the <see cref="Type"/> of a <see cref="MemberInfo"/>.
		/// TODO: works only with properties and fields.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		private static System.Type GetType(MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)member).FieldType;

				case MemberTypes.Property:
					return ((PropertyInfo)member).PropertyType;
				default:
					throw new ArgumentException("The argument must be a property or field", "member");
			}
		}


		/// <summary>
		/// Get the value of some Property or Field.
		/// TODO: refactor this to some Utils.
		/// </summary>
		/// <param name="bean"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		private static object GetMemberValue(object bean, MemberInfo member)
		{
			FieldInfo fi = member as FieldInfo;
			if (fi != null)
				return fi.GetValue(bean);

			PropertyInfo pi = member as PropertyInfo;
			if (pi != null)
				return pi.GetValue(bean, ReflectHelper.AnyVisibilityInstance | BindingFlags.GetProperty, null, null, null);

			return null;
		}

		/// <summary>
		/// Add recursively the inheritance tree of types (Classes and Interfaces)
		/// to the parameter <paramref name="classes"/>
		/// </summary>
		/// <param name="clazz">Type to be analyzed</param>
		/// <param name="classes">Collections of types</param>
		private static void AddSuperClassesAndInterfaces(System.Type clazz, ISet<System.Type> classes)
		{
			//iterate for all SuperClasses
			for (System.Type currentClass = clazz; currentClass != null; currentClass = currentClass.BaseType)
			{
				if (!classes.Add(clazz))
				{
					return; //Base case for the recursivity
				}

				System.Type[] interfaces = currentClass.GetInterfaces();

				foreach (System.Type @interface in interfaces)
				{
					AddSuperClassesAndInterfaces(@interface, classes);
				}
			}
		}

		/// <summary>
		/// Assert a valid Object. A <see cref="InvalidStateException"/> 
		/// will be throw in a Invalid state.
		/// </summary>
		/// <param name="bean">Object to be asserted</param>
		public void AssertValid(object bean)
		{
			InvalidValue[] values = GetInvalidValues(bean);
			if (values.Length > 0)
			{
				throw new InvalidStateException(values);
			}
		}

		/// <summary>
		/// Apply constraints of a particular property value of a bean type and return all the failures.
		/// The InvalidValue objects returns return null for InvalidValue#getBean() and InvalidValue#getRootBean()
		/// Note: this is not recursive.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public InvalidValue[] GetPotentialInvalidValues(string propertyName, object value)
		{
			List<InvalidValue> results = new List<InvalidValue>();

			for (int i = 0; i < memberValidators.Count; i++)
			{
				MemberInfo getter = memberGetters[i];
				if (getter.Name.Equals(propertyName))
				{
					IValidator validator = memberValidators[i];
					if (!validator.IsValid(value))
						results.Add(new InvalidValue(Interpolate(validator), beanClass, propertyName, value, null));
				}
			}

			return results.ToArray();
		}

		/// <summary>
		/// Apply the registred constraints rules on the hibernate metadata (to be applied on DB schema)
		/// </summary>
		/// <param name="persistentClass">hibernate metadata</param>
		public void Apply(PersistentClass persistentClass)
		{
			foreach (IValidator validator in beanValidators)
			{
				if (validator is IPersistentClassConstraint)
					((IPersistentClassConstraint)validator).Apply(persistentClass);
			}

			for (int i = 0; i < memberValidators.Count; i++)
			{
				IValidator validator = memberValidators[i];
				MemberInfo getter = memberGetters[i];

				string propertyName = getter.Name;

				if (validator is IPropertyConstraint)
				{
					try
					{
						Property property = FindPropertyByName(persistentClass, propertyName);
						if (property != null)
							((IPropertyConstraint)validator).Apply(property);
					}
					catch (MappingException)
					{
					}
				}
			}
		}

		private static Property FindPropertyByName(PersistentClass associatedClass, string propertyName)
		{
			Property property = null;
			Property idProperty = associatedClass.IdentifierProperty;
			string idName = idProperty != null ? idProperty.Name : null;
			try
			{
				//if it's a Id
				if (propertyName == null || propertyName.Length == 0 || propertyName.Equals(idName))
					property = idProperty;
				else //if it's a property
				{
					if (propertyName.IndexOf(idName + ".") == 0)
					{
						property = idProperty;
						propertyName = propertyName.Substring(idName.Length + 1);
					}

					foreach (string element in new StringTokenizer(propertyName, ".", false))
					{
						if (property == null)
							property = associatedClass.GetProperty(element);
						else
						{
							if (property.IsComposite)
								property = ((Component)property.Value).GetProperty(element);
							else
								return null;
						}
					}
				}
			}
			catch (MappingException)
			{
				try
				{
					//if we do not find it try to check the identifier mapper
					if (associatedClass.IdentifierMapper == null) return null;
					StringTokenizer st = new StringTokenizer(propertyName, ".", false);

					foreach (string element in st)
					{
						if (property == null)
						{
							property = associatedClass.IdentifierMapper.GetProperty(element);
						}
						else
						{
							if (property.IsComposite)
								property = ((Component)property.Value).GetProperty(element);
							else
								return null;
						}
					}
				}
				catch (MappingException)
				{
					return null;
				}

			}

			return property;
		}
	}
}