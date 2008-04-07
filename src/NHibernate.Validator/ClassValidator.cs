using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using NHibernate.Validator.Util;
using NHibernate.Validator.XmlConfiguration;
using NHibernate.Validator.Cfg;

namespace NHibernate.Validator
{
	/// <summary>
	/// Engine that take a object and check every expressed attribute restrictions
	/// </summary>
	[Serializable]
	public class ClassValidator : IClassValidator
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ClassValidator));

		private readonly System.Type beanClass;

		private static Dictionary<AssemblyQualifiedTypeName, NhvClass> validatorMappings;
		private Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary = new Dictionary<MemberInfo,List<Attribute>>();

		private DefaultMessageInterpolatorAggregator defaultInterpolator;

		[NonSerialized]
		private readonly ResourceManager messageBundle;

		[NonSerialized]
		private readonly ResourceManager defaultMessageBundle;

		[NonSerialized]
		private readonly IMessageInterpolator userInterpolator;

		private readonly Dictionary<System.Type, ClassValidator> childClassValidators;

		private static Dictionary<System.Type, IClassValidator> polimorphicClasses;

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
			SetXmlValidators(clazz.Assembly, validatorMode);

			//Initialize the ClassValidator
			InitValidator(beanClass, childClassValidators);
		}

		private void SetXmlValidators(Assembly assembly, ValidatorMode validatorMode)
		{
			if (CfgXmlHelper.ModeAcceptsXML(validatorMode) && validatorMappings == null)
			{
				validatorMappings = new Dictionary<AssemblyQualifiedTypeName, NhvClass>();
				GetAllNHVXmlResourceNames(assembly);
			}
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

		private static Dictionary<System.Type, IClassValidator> PolimorphicClasses
		{
			get 
			{
				if (polimorphicClasses == null)
					polimorphicClasses = new Dictionary<System.Type, IClassValidator>();

				return polimorphicClasses; 
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
				// TODO: Support override in class Attributes
				if (validatorMode == ValidatorMode.UseXml)
				{
					CreateClassMembersFromXml(currentClass);
				}
				else
				{
					foreach (Attribute classAttribute in currentClass.GetCustomAttributes(false))
					{
						ValidateClassAtribute(classAttribute);
					}
				}
			}

			//Check on all selected classes
			foreach (System.Type currentClass in classes)
			{
				if (CfgXmlHelper.ModeAcceptsXML(validatorMode))
				{
					CreateAttributesFromXml(currentClass);
				}

				if (CfgXmlHelper.ModeAcceptsAttributes(validatorMode))
				{
					foreach (MemberInfo member in currentClass.GetMembers(ReflectHelper.AnyVisibilityInstance | BindingFlags.Static))
					{
						CreateMemberAttributes(member);
						CreateChildValidator(member);
					}
				}
			}

			foreach (MemberInfo member in membersAttributesDictionary.Keys)
			{
				foreach (Attribute memberAttribute in membersAttributesDictionary[member])
				{
					IValidator propertyValidator = CreateValidator(memberAttribute);

					if (propertyValidator != null)
					{
						memberValidators.Add(propertyValidator);
						memberGetters.Add(member);
					}
				}
			}
		}

		private void AddAttributeToMember(MemberInfo currentMember, Attribute thisattribute, bool overrideAttribute)
		{
			log.Info(string.Format("Adding member {0} to dictionary with attribute {1}", currentMember.Name, thisattribute.ToString()));
			if (!membersAttributesDictionary.ContainsKey(currentMember))
			{
				membersAttributesDictionary.Add(currentMember, new List<Attribute>());
			}
			else
			{
				bool exists = membersAttributesDictionary[currentMember].Exists(
					delegate(Attribute theattribute)
					{
						return thisattribute.ToString() == theattribute.ToString();
					});

				if (exists && !AttributeUtils.AttributeAllowsMultiple(thisattribute))
				{
					log.Info(string.Format("Attribute {0} was found , override was {1}", thisattribute.ToString(), overrideAttribute));
					// If we cannot override then exit without changing
					if (!overrideAttribute)
						return;

					membersAttributesDictionary[currentMember].Remove(thisattribute);
				}
			}

			membersAttributesDictionary[currentMember].Add(thisattribute);
		}

		private void ValidateClassAtribute(Attribute classAttribute)
		{
			IValidator validator = CreateValidator(classAttribute);

			if (validator != null)
			{
				beanValidators.Add(validator);
			}

			//Note: No need to handle Aggregate annotations, c# use Multiple Attribute declaration.
			//HandleAggregateAnnotations(classAttribute, null);
		}

		private void CreateClassMembersFromXml(System.Type currentClass)
		{
			NhvClass clazz = GetNhvClassFor(currentClass);
			if (clazz == null || clazz.attributename == null) return;
			log.Debug("Looking for class attributes");
			foreach (string attributeName in clazz.attributename)
			{
				log.Info("Attribute to look for = " + attributeName);
				Attribute classAttribute = XmlRulesFactory.CreateAttributeFromClass(currentClass, attributeName);
				if (classAttribute != null)
				{
					ValidateClassAtribute(classAttribute);
				}
			}
		}

		private void GetAllNHVXmlResourceNames(Assembly assembly)
		{
			foreach (string resource in assembly.GetManifestResourceNames())
			{
				if (resource.EndsWith(".nhv.xml"))
				{
					AddNhvClasses(assembly, resource);
				}
			}
		}

		private void AddNhvClasses(Assembly assembly, string resource)
		{
			IMappingDocumentParser parser = new MappingDocumentParser();
			
			NhvValidator validator = parser.Parse(assembly.GetManifestResourceStream(resource));
			foreach (NhvClass clazz in validator.@class)
			{
				AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(clazz.name, clazz.@namespace, clazz.assembly);
				log.Info("Full class name = " + fullClassName);
				if (!validatorMappings.ContainsKey(fullClassName))
				{
					validatorMappings.Add(fullClassName, clazz);
				}
			}
		}

		private void CreateAttributesFromXml(System.Type currentClass)
		{
			NhvClass clazz = GetNhvClassFor(currentClass);
			if (clazz == null || clazz.property == null) return;
			
			foreach (NhvProperty property in clazz.property)
			{
				MemberInfo currentMember = TypeUtils.GetPropertyOrField(currentClass, property.name);

				if (currentMember == null)
				{
					log.Error(string.Format("Property or field \"{0}\" was not found in the class: \"{1}\" ", property.name, currentClass.FullName));
					continue;
				}
				log.Info("Looking for rules for property : " + property.name);
				CreateMemberAttributesFromRules(currentMember, property.rules);
				CreateChildValidator(currentMember);
			}
		}

		private NhvClass GetNhvClassFor(System.Type currentClass)
		{
			AssemblyQualifiedTypeName fullClassName = TypeNameParser.Parse(currentClass.Name, currentClass.Namespace, currentClass.Assembly.GetName().Name);

			log.Info("Looking for class name = " + fullClassName);

			if (validatorMappings.ContainsKey(fullClassName))
			{
				return validatorMappings[fullClassName];
			}

			return null;
		}

		private void CreateMemberAttributesFromRules(MemberInfo member, NhvRules rules)
		{
			foreach (object rule in rules.Items)
			{
				Attribute thisAttribute = XmlRulesFactory.CreateAttributeFromRule(rule);

				if (thisAttribute != null)
				{
					AddAttributeToMember(member, thisAttribute, validatorMode == ValidatorMode.OverrideAttributeWithXml);
				}
			}
		}

		/// <summary>
		/// apply constraints on a bean instance and return all the failures.
		/// if <see cref="bean"/> is null, an empty array is returned 
		/// </summary>
		/// <param name="bean">object to apply the constraints</param>
		/// <returns></returns>
		public InvalidValue[] GetInvalidValues(object bean)
		{
			System.Type type = bean.GetType();

			if(!type.Equals(beanClass) && (type.IsSubclassOf(beanClass)|| TypeUtils.IsImplementationOf(type,beanClass)))
			{
				return HandlePolimorphicBehaviour(bean, type);
			}

			return GetInvalidValues(bean, new IdentitySet());
		}
		
		private InvalidValue[] HandlePolimorphicBehaviour(object bean, System.Type type)
		{
			log.WarnFormat("the type of the object to validate is {0}, but the ClassValidator was declared as {1}",type.FullName,beanClass.FullName);

			IClassValidator polimorphicValidator;

			if (PolimorphicClasses.ContainsKey(type))
			{
				polimorphicValidator = PolimorphicClasses[type];
			}
			else
			{
				polimorphicValidator =
					new ClassValidator(
						bean.GetType(),
						defaultMessageBundle,
						culture,
						userInterpolator,
						new Dictionary<System.Type, ClassValidator>(),
						validatorMode);

				//recycling this construction.
				PolimorphicClasses.Add(type, polimorphicValidator);
			}

			return polimorphicValidator.GetInvalidValues(bean);
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
					object value = TypeUtils.GetMemberValue(bean, member);

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
					object value = TypeUtils.GetMemberValue(bean, member);

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
			if (TypeUtils.IsGenericDictionary(value.GetType())) //Generic Dictionary
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
			catch (HibernateValidatorException ex)
			{
				throw new HibernateValidatorException("could not instantiate ClassValidator, maybe some validator is not well formed", ex);
			}
			catch (Exception ex)
			{
				throw new HibernateValidatorException("could not instantiate ClassValidator", ex);
			}
		}

		private void CreateMemberAttributes(MemberInfo member)
		{
			object[] memberAttributes = member.GetCustomAttributes(false);

			foreach (Attribute memberAttribute in memberAttributes)
			{
				AddAttributeToMember(member, memberAttribute, validatorMode== ValidatorMode.OverrideXmlWithAttribute);
			}
		}

		/// <summary>
		/// Create a Validator from a property or field.
		/// </summary>
		/// <param name="member"></param>
		//private void CreateMemberValidator(MemberInfo member)
		//{
		//    object[] memberAttributes = member.GetCustomAttributes(false);

		//    foreach (Attribute memberAttribute in memberAttributes)
		//    {
		//        IValidator propertyValidator = CreateValidator(memberAttribute);

		//        if (propertyValidator != null)
		//        {
		//            memberValidators.Add(propertyValidator);
		//            memberGetters.Add(member);
		//        }
		//    }
		//}

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

			if (TypeUtils.IsGenericDictionary(TypeUtils.GetType(member)))
			{
				clazzDictionary = TypeUtils.GetGenericTypesOfDictionary(member);
				if (!childClassValidators.ContainsKey(clazzDictionary.Key))
					new ClassValidator(clazzDictionary.Key, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);
				if (!childClassValidators.ContainsKey(clazzDictionary.Value))
					new ClassValidator(clazzDictionary.Value, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);

				return;
			}
			else
			{
				clazz = TypeUtils.GetTypeOfMember(member);
			}

			if (!childClassValidators.ContainsKey(clazz))
			{
				new ClassValidator(clazz, messageBundle, culture, userInterpolator, childClassValidators, validatorMode);
			}
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