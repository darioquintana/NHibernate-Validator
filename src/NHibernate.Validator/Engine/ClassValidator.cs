using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using Iesi.Collections;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Util;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Interpolator;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Util;
using Environment=NHibernate.Validator.Cfg.Environment;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Engine that take a object and check every expressed attribute restrictions
	/// </summary>
	[Serializable]
	public class ClassValidator : IClassValidator, IClassValidatorImplementor, ISerializable 
	{
		[Serializable]
		private class Member
		{
			public ValidatorDef ValidatorDef { get; set; }
			public MemberInfo Getter { get; set; }
		}
		private static readonly ILog log = LogManager.GetLogger(typeof(ClassValidator));
		private readonly System.Type entityType;
		private readonly IConstraintValidatorFactory constraintValidatorFactory;

		private readonly Dictionary<MemberInfo, List<Attribute>> membersAttributesDictionary =
			new Dictionary<MemberInfo, List<Attribute>>();

		private DefaultMessageInterpolatorAggregator defaultInterpolator;

		[NonSerialized]
		private readonly ResourceManager messageBundle;

		[NonSerialized]
		private readonly ResourceManager defaultMessageBundle;

		[NonSerialized]
		private readonly IMessageInterpolator userInterpolator;

		/// <summary>
		/// Used to create an instance when deserialize
		/// </summary>
		private readonly System.Type userInterpolatorType; 

		[NonSerialized]
		private readonly IClassValidatorFactory factory;

		private readonly IDictionary<System.Type, IClassValidator> childClassValidators;

		private IList<ValidatorDef> entityValidators;

		private List<Member> membersToValidate;

		private List<MemberInfo> childGetters;

		public static readonly InvalidValue[] EMPTY_INVALID_VALUE_ARRAY = new InvalidValue[0];
		public static readonly IEnumerable<Attribute> EmptyConstraints = new Attribute[0];

		private readonly CultureInfo culture;

		private readonly ValidatorMode validatorMode;


		/// <summary>
		/// Create the validator engine for this entity type. 
		/// </summary>
		/// <remarks>
		/// Used in Unit Testing.
		/// </remarks>
		/// <param name="entityType"></param>
		public ClassValidator(System.Type entityType)
			: this(entityType, null, null, ValidatorMode.UseAttribute) {}

		/// <summary>
		/// Create the validator engine for a particular entity class, using a resource bundle
		/// for message rendering on violation
		/// </summary>
		/// <remarks>
		/// Used in Unit Testing.
		/// </remarks>
		/// <param name="entityType">entity type</param>
		/// <param name="resourceManager"></param>
		/// <param name="culture">The CultureInfo for the <paramref name="entityType"/>.</param>
		/// <param name="validatorMode">Validator definition mode</param>
		public ClassValidator(System.Type entityType, ResourceManager resourceManager, CultureInfo culture,
		                      ValidatorMode validatorMode)
			: this(
				entityType, new DefaultConstraintValidatorFactory(), new Dictionary<System.Type, IClassValidator>(),
				new JITClassValidatorFactory(new DefaultConstraintValidatorFactory(), resourceManager, culture, null, validatorMode,
				                             new DefaultEntityTypeInspector())) {}

		/// <summary>
		/// Create the validator engine for a particular entity class, using a resource bundle
		/// for message rendering on violation
		/// </summary>
		/// <remarks>
		/// Used in Unit Testing.
		/// </remarks>
		/// <param name="entityType">entity type</param>
		/// <param name="resourceManager"></param>
		/// <param name="userInterpolator">Custom interpolator.</param>
		/// <param name="culture">The CultureInfo for the <paramref name="entityType"/>.</param>
		/// <param name="validatorMode">Validator definition mode</param>
		public ClassValidator(System.Type entityType, ResourceManager resourceManager, IMessageInterpolator userInterpolator,
		                      CultureInfo culture, ValidatorMode validatorMode)
			: this(
				entityType, new DefaultConstraintValidatorFactory(), new Dictionary<System.Type, IClassValidator>(),
				new JITClassValidatorFactory(new DefaultConstraintValidatorFactory(), resourceManager, culture, userInterpolator,
				                             validatorMode, new DefaultEntityTypeInspector())) {}

		internal ClassValidator(System.Type clazz, IConstraintValidatorFactory constraintValidatorFactory,
		                        IDictionary<System.Type, IClassValidator> childClassValidators, IClassValidatorFactory factory)
		{
			if (!ShouldNeedValidation(clazz))
			{
				throw new ArgumentOutOfRangeException("clazz", "Create a validator for a System class.");
			}

			entityType = clazz;
			this.constraintValidatorFactory = constraintValidatorFactory;
			this.factory = factory;
			messageBundle = factory.ResourceManager ?? GetDefaultResourceManager();
			defaultMessageBundle = GetDefaultResourceManager();
			culture = factory.Culture;
			userInterpolator = factory.UserInterpolator;
			if (userInterpolator != null)
			{
				userInterpolatorType = factory.UserInterpolator.GetType();
			}
			this.childClassValidators = childClassValidators;
			validatorMode = factory.ValidatorMode;

			//Initialize the ClassValidator
			InitValidator(entityType, childClassValidators);
		}

		internal static bool ShouldNeedValidation(System.Type clazz)
		{
			return (!clazz.FullName.StartsWith("System") &&
			        !clazz.IsValueType);
		}

		/// <summary>
		/// Return true if this <see cref="ClassValidator"/> contains rules for apply, false in other case. 
		/// </summary>
		public bool HasValidationRules
		{
			get
			{
				return membersToValidate.Count != 0 || entityValidators.Count != 0 || childClassValidators.Count > 1;
			}
		}

		private static ResourceManager GetDefaultResourceManager()
		{
			return new ResourceManager(Environment.BaseNameOfMessageResource, Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// Initialize the <see cref="ClassValidator"/> type.
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="nestedClassValidators"></param>
		private void InitValidator(System.Type clazz, IDictionary<System.Type, IClassValidator> nestedClassValidators)
		{
			entityValidators = new List<ValidatorDef>();
			membersToValidate = new List<Member>();
			childGetters = new List<MemberInfo>();
			defaultInterpolator = new DefaultMessageInterpolatorAggregator();
			defaultInterpolator.Initialize(messageBundle, defaultMessageBundle, culture);

			//build the class hierarchy to look for members in
			nestedClassValidators.Add(clazz, this);
			ISet<System.Type> classes = new HashedSet<System.Type>();
			AddSuperClassesAndInterfaces(clazz, classes);
			
			// Create the IClassMapping for each class of the validator
			var classesMaps = new List<IClassMapping>(classes.Count);
			foreach (System.Type type in classes)
			{
				IClassMapping mapping = factory.ClassMappingFactory.GetClassMapping(type, validatorMode);
				if (mapping != null)
					classesMaps.Add(mapping);
				else
					log.Warn("Validator not found in mode " + validatorMode + " for class " + clazz.AssemblyQualifiedName);
			}

			//Check on all selected classes
			foreach (IClassMapping map in classesMaps)
			{
				foreach (Attribute classAttribute in map.GetClassAttributes())
				{
					ValidateClassAtribute(classAttribute);
				}

				foreach (MemberInfo member in map.GetMembers())
				{
					var memberAttributes = map.GetMemberAttributes(member);
					CreateMemberAttributes(member, memberAttributes);
					CreateChildValidator(member, memberAttributes);

					foreach (Attribute memberAttribute in memberAttributes)
					{
						IValidator propertyValidator = CreateValidator(memberAttribute);

						if (propertyValidator != null)
						{
							var tagable = memberAttribute as ITagableRule;
							membersToValidate.Add(new Member
							                      	{
							                      		ValidatorDef =
							                      			new ValidatorDef(propertyValidator, tagable != null ? tagable.TagCollection : null),
							                      		Getter = member
							                      	});
						}
					}
				}
			}
		}

		private void AddAttributeToMember(MemberInfo currentMember, Attribute thisattribute)
		{
			log.Info(string.Format("Adding member {0} to dictionary with attribute {1}", currentMember.Name, thisattribute));
			if (!membersAttributesDictionary.ContainsKey(currentMember))
			{
				membersAttributesDictionary.Add(currentMember, new List<Attribute>());
			}

			membersAttributesDictionary[currentMember].Add(thisattribute);
		}

		private void ValidateClassAtribute(Attribute classAttribute)
		{
			IValidator validator = CreateValidator(classAttribute);

			if (validator != null)
			{
				var tagable = classAttribute as ITagableRule;
				entityValidators.Add(new ValidatorDef(validator, tagable != null ? tagable.TagCollection : null));
			}

			//Note: No need to handle Aggregate annotations, c# use Multiple Attribute declaration.
			//HandleAggregateAnnotations(classAttribute, null);
		}

		/// <summary>
		/// Apply constraints on a entity instance and return all the failures.
		/// if <paramref name="entity"/> is null, an empty array is returned 
		/// </summary>
		/// <param name="entity">object to apply the constraints</param>
		/// <returns></returns>
		public IEnumerable<InvalidValue> GetInvalidValues(object entity)
		{
			return GetInvalidValues(entity, new IdentitySet(), null);
		}

		public IEnumerable<InvalidValue> GetInvalidValues(object entity, string propertyName)
		{
			return GetInvalidValues(entity, propertyName, null);
		}

		private IEnumerable<InvalidValue> GetInvalidValues(object entity, ISet circularityState, ICollection<object> activeTags)
		{
			if (entity == null || circularityState.Contains(entity))
			{
				return EMPTY_INVALID_VALUE_ARRAY; //Avoid circularity
			}
			circularityState.Add(entity);

			if (!entityType.IsInstanceOfType(entity))
			{
				throw new ArgumentException("not an instance of: " + entity.GetType());
			}
			return 
				EntityInvalidValues(entity, activeTags)
				.Concat(MembersInvalidValues(entity, null, activeTags))
				.Concat(ChildrenInvalidValues(entity, circularityState, activeTags));
		}

		private IEnumerable<InvalidValue> ChildrenInvalidValues(object entity, ISet circularityState,
		                                                        ICollection<object> activeTags)
		{
			return from member in childGetters
			       where NHibernateUtil.IsPropertyInitialized(entity, member.Name)
			       let value = TypeUtils.GetMemberValue(entity, member)
			       where value != null && NHibernateUtil.IsInitialized(value)
			       from invalidValue in ChildInvalidValues(value, entity, member, circularityState, activeTags)
			       select invalidValue;
		}

		private IEnumerable<InvalidValue> EntityInvalidValues(object entity, ICollection<object> activeTags)
		{
			return from validator in entityValidators.Where(ev => IsValidationNeededByTags(activeTags, ev.Tags)).Select(ev => ev.Validator)
						 let constraintContext = new ConstraintValidatorContext(null, defaultInterpolator.GetAttributeMessage(validator))
						 where !validator.IsValid(entity, constraintContext)
						 select new InvalidMessageTransformer(constraintContext, entityType, null, entity, entity, validator, defaultInterpolator, userInterpolator)
							 into invalidMessageTransformer
							 from invalidValue in invalidMessageTransformer.Transform()
							 select invalidValue;
		}

		private IEnumerable<InvalidValue> MembersInvalidValues(object entity, string memberName, ICollection<object> activeTags)
		{
			// Property & Field Validation
			var membersValidators =
				membersToValidate.Where(mtv => memberName == null || mtv.Getter.Name.Equals(memberName)).ToArray();
			if (memberName != null && membersValidators.Length == 0 && TypeUtils.GetPropertyOrField(entityType, memberName) == null)
			{
				throw new TargetException(
					string.Format("The property or field '{0}' was not found in class {1}", memberName, entityType.FullName));
			}

			return from mtv in membersValidators
			       let member = mtv.Getter
			       where NHibernateUtil.IsPropertyInitialized(entity, member.Name)
			       let value = TypeUtils.GetMemberValue(entity, member)
						 where IsValidationNeededByTags(activeTags, mtv.ValidatorDef.Tags) && NHibernateUtil.IsInitialized(value)
						 let validator = mtv.ValidatorDef.Validator
			       let constraintContext = new ConstraintValidatorContext(member.Name, defaultInterpolator.GetAttributeMessage(validator))
			       where !validator.IsValid(value, constraintContext)
			       from invalidValue in new InvalidMessageTransformer(constraintContext, entityType, member.Name, value, entity, validator, defaultInterpolator, userInterpolator).Transform()
			       select invalidValue;
		}

		private bool IsValidationNeededByTags(ICollection<object> activeTags, ICollection<object> validatorTags)
		{
			return activeTags == null || 
				((validatorTags == null || validatorTags.Count == 0) 
					&& activeTags.Contains(null)) || 
				activeTags.Where(at=> validatorTags!=null && validatorTags.Contains(at)).Any();
		}

		/// <summary>
		/// Validate the child validation to objects and collections
		/// </summary>
		private IEnumerable<InvalidValue> ChildInvalidValues(object value, object entity, MemberInfo member, ISet circularityState, ICollection<object> activeTags)
		{
			var valueEnum = value as IEnumerable;
			if (valueEnum != null)
			{
				return TypeUtils.IsGenericDictionary(value.GetType())
				       	? DictionaryInvalidValues(valueEnum, member, entity, circularityState, activeTags)
				       	: CollectionInvalidValues(valueEnum, entity, member, circularityState, activeTags);
			}
			else
			{
				//Simple Value, Non-Collection
				return GetClassValidator(GuessEntityType(value)).GetInvalidValues(value, circularityState, activeTags).WithParent(entity, member.Name);
			}
		}

		private System.Type GuessEntityType(object value)
		{
			return factory.EntityTypeInspector.GuessType(value) ?? value.GetType();
		}

		private IEnumerable<InvalidValue> CollectionInvalidValues(IEnumerable value, object entity, MemberInfo member, ISet circularityState, ICollection<object> activeTags)
		{
			return
				value.Cast<object>().Select(
					(item, i) => new {Item = item, IndexedPropName = string.Format("{0}[{1}]", member.Name, i)}).Where(
					elem => elem.Item != null).SelectMany(
					elem => CollectionItemInvalidValues(entity, elem.IndexedPropName, elem.Item, circularityState, activeTags));
		}

		private IEnumerable<InvalidValue> CollectionItemInvalidValues(object collectionOwner, string indexedPropName, object item, ISet circularityState, ICollection<object> activeTags)
		{
			if(item == null)
			{
				return EMPTY_INVALID_VALUE_ARRAY;
			}
			System.Type candidateType = factory.EntityTypeInspector.GuessType(item);
			System.Type itemType = candidateType ?? item.GetType();
			if (!ShouldNeedValidation(itemType))
			{
				return EMPTY_INVALID_VALUE_ARRAY;
			}
			return GetClassValidator(itemType).GetInvalidValues(item, circularityState, activeTags).WithParent(collectionOwner,
			                                                                                                   indexedPropName);
		}

		private IEnumerable<InvalidValue> DictionaryInvalidValues(IEnumerable dictionary, MemberInfo dictionaryMember, object ownerEntity, ISet circularityState, ICollection<object> activeTags)
		{
			return dictionary.AsKeyValue()
				.Select(item => new {item, indexedPropName = string.Format("{0}[{1}]", dictionaryMember.Name, item.Key)})
				.SelectMany(t => CollectionItemInvalidValues(ownerEntity, t.indexedPropName, t.item.Value, circularityState, activeTags)
					.Concat(CollectionItemInvalidValues(ownerEntity, t.indexedPropName, t.item.Key, circularityState, activeTags)));
		}

		/// <summary>
		/// Get the ClassValidator for the <paramref name="objectType"/>
		/// parametter  from <see cref="childClassValidators"/>. If doesn't exist, a 
		/// new <see cref="ClassValidator"/> is returned.
		/// </summary>
		/// <param name="objectType">type</param>
		/// <returns></returns>
		private IClassValidatorImplementor GetClassValidator(System.Type objectType)
		{
			IClassValidator result;
			if (!childClassValidators.TryGetValue(objectType, out result))
				return (factory.GetRootValidator(objectType) as IClassValidatorImplementor);
			else
				return (result as IClassValidatorImplementor);
		}

		/// <summary>
		/// Create a <see cref="IValidator"/> from a <see cref="ValidatorClassAttribute"/> attribute.
		/// If the attribute is not a <see cref="ValidatorClassAttribute"/> type return null.
		/// </summary>
		/// <param name="attribute">attribute</param>
		/// <returns>the validator for the attribute</returns>
		private IValidator CreateValidator(Attribute attribute)
		{
			try
			{
				IValidator entityValidator;
				var delegatedAttribute = attribute as IValidatorInstanceProvider;
				if (delegatedAttribute != null)
				{
					entityValidator = delegatedAttribute.Validator;
				}
				else
				{
					ValidatorClassAttribute validatorClass = null;
					object[] attributesInTheAttribute = attribute.GetType().GetCustomAttributes(typeof (ValidatorClassAttribute), false);

					if (attributesInTheAttribute.Length > 0)
					{
						validatorClass = (ValidatorClassAttribute) attributesInTheAttribute[0];
					}

					if (validatorClass == null)
					{
						return null;
					}

					entityValidator = constraintValidatorFactory.GetInstance(validatorClass.Value);
					InitializeValidator(attribute, validatorClass.Value, entityValidator);
				}

				defaultInterpolator.AddInterpolator(attribute, entityValidator);
				return entityValidator;
			}
			catch (Exception ex)
			{
				throw new HibernateValidatorException(
					"could not instantiate ClassValidator, maybe some validator is not well formed; check InnerException", ex);
			}
		}

		private static readonly System.Type BaseInitializableType = typeof(IInitializableValidator<>);
		private static void InitializeValidator(Attribute attribute, System.Type validatorClass, IValidator entityValidator)
		{
			/* This method was added to supply major difference between JAVA and NET generics.
			 * So far in JAVA the generic type is something optional, in NET mean "strongly typed".
			 * In this case we don't know exactly wich is the generic version of "IInitializableValidator<>"
			 * so we create the type on the fly and invoke the Initialize method by reflection.
			 * All this work is only to give to the user the ability of implement IInitializableValidator<>
			 * without need to inherit from a base class or implement a generic and a no generic version of
			 * the method Initialize.
			 */
			System.Type[] args = {attribute.GetType()};
			System.Type concreteIvc = BaseInitializableType.MakeGenericType(args);
			if (concreteIvc.IsAssignableFrom(validatorClass))
			{
				MethodInfo initMethod = concreteIvc.GetMethod("Initialize");
				initMethod.Invoke(entityValidator, new object[] {attribute});
			}
		}

		private void CreateMemberAttributes(MemberInfo member, IEnumerable<Attribute> memberAttributes)
		{
			foreach (Attribute memberAttribute in memberAttributes)
			{
				AddAttributeToMember(member, memberAttribute);
			}
		}

		/// <summary>
		/// Create the validator for the children, who got the <see cref="ValidAttribute"/>
		/// on the fields or properties
		/// </summary>
		private void CreateChildValidator(MemberInfo member, IEnumerable<Attribute> memberAttributes)
		{
			if (memberAttributes.OfType<ValidAttribute>().FirstOrDefault() == null) return;

			KeyValuePair<System.Type, System.Type> clazzDictionary;
			System.Type clazz;

			childGetters.Add(member);

			if (TypeUtils.IsGenericDictionary(TypeUtils.GetType(member)))
			{
				clazzDictionary = TypeUtils.GetGenericTypesOfDictionary(member);
				if (ShouldNeedValidation(clazzDictionary.Key) && !childClassValidators.ContainsKey(clazzDictionary.Key))
					factory.GetChildValidator(this, clazzDictionary.Key);
				if (ShouldNeedValidation(clazzDictionary.Value) && !childClassValidators.ContainsKey(clazzDictionary.Value))
					factory.GetChildValidator(this, clazzDictionary.Value);

				return;
			}
			else
			{
				clazz = TypeUtils.GetTypeOfMember(member);
			}

			if (ShouldNeedValidation(clazz) && !childClassValidators.ContainsKey(clazz))
			{
				factory.GetChildValidator(this, clazz);
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
			for (System.Type currentClass = clazz; currentClass != null && currentClass != typeof(object); currentClass = currentClass.BaseType)
			{
				if (!classes.Add(currentClass))
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
		/// <param name="entity">Object to be asserted</param>
		public void AssertValid(object entity)
		{
			var invalidValues = GetInvalidValues(entity);
			if (invalidValues.Any())
			{
				throw new InvalidStateException(invalidValues.ToArray());
			}
		}

		/// <summary>
		/// Apply constraints of a particular property value of a entity type and return all the failures.
		/// The InvalidValue objects returns an empty enumerable for InvalidValue#Entity and InvalidValue#RootEntity.
		/// Note: this is not recursive.
		/// </summary>
		/// <param name="propertyName">Name of the property or field to validate</param>
		/// <param name="value">Real value to validate. Is not an entity instance.</param>
		/// <returns></returns>
		public IEnumerable<InvalidValue> GetPotentialInvalidValues(string propertyName, object value)
		{
			return GetPotentialInvalidValues(propertyName, value, null);
		}

		/// <summary>
		/// Apply the registred constraints rules on the hibernate metadata (to be applied on DB schema)
		/// </summary>
		/// <param name="persistentClass">hibernate metadata</param>
		public void Apply(PersistentClass persistentClass)
		{
			foreach (var pcc in entityValidators.Select(ev=>ev.Validator).OfType<IPersistentClassConstraint>())
			{
				pcc.Apply(persistentClass);
			}

			foreach (Member member in membersToValidate)
			{
				var pc = member.ValidatorDef.Validator as IPropertyConstraint;
				if (pc != null)
				{
					Property property = FindPropertyByName(persistentClass, member.Getter.Name);
					if (property != null)
						pc.Apply(property);
				}
			}
		}

		public IEnumerable<Attribute> GetMemberConstraints(string propertyName)
		{
			return membersAttributesDictionary.Where(member => member.Key.Name.Equals(propertyName)).SelectMany(member => member.Value);
		}

		public IEnumerable<InvalidValue> GetInvalidValues(object entity, params object[] tags)
		{
			return GetInvalidValues(entity, new IdentitySet(), tags != null ? new HashSet<object>(tags) : null);
		}

		public IEnumerable<InvalidValue> GetInvalidValues(object entity, string propertyName, params object[] tags)
		{
			if (entity == null)
			{
				return EMPTY_INVALID_VALUE_ARRAY;
			}
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}
			if (!entityType.IsInstanceOfType(entity))
			{
				throw new ArgumentException("not an instance of: " + entity.GetType());
			}
			return MembersInvalidValues(entity, propertyName, tags != null ? new HashSet<object>(tags) : null);
		}

		public IEnumerable<InvalidValue> GetPotentialInvalidValues(string propertyName, object value, params object[] tags)
		{
			var activeTags = tags != null ? new HashSet<object>(tags) : null;
			var memberValidators = membersToValidate.Where(m => m.Getter.Name.Equals(propertyName) && IsValidationNeededByTags(activeTags, m.ValidatorDef.Tags)).ToArray();
			if (memberValidators.Length == 0)
			{
				throw new TargetException(string.Format("The property or field '{0}' was not found in class {1}", propertyName,
																								entityType.FullName));
			}
			return from member in memberValidators
						 select member.ValidatorDef.Validator
							 into validator
							 let constraintContext =
							 new ConstraintValidatorContext(propertyName, defaultInterpolator.GetAttributeMessage(validator))
							 where !validator.IsValid(value, constraintContext)
							 from invalidValue in new InvalidMessageTransformer(constraintContext, entityType, propertyName, value, null, validator, defaultInterpolator, userInterpolator).Transform()
							 select invalidValue;
		}

		private static Property FindPropertyByName(PersistentClass associatedClass, string propertyName)
		{
			Property property;
			Property idProperty = associatedClass.IdentifierProperty;
			string idName = idProperty != null ? idProperty.Name : null;
			try
			{
				//if it's a Id
				if (string.IsNullOrEmpty(propertyName) || propertyName.Equals(idName))
					property = idProperty;
				else //if it's a property
				{
					property = associatedClass.GetProperty(propertyName);
				}
			}
			catch (MappingException)
			{
				#region Unsupported future of NH2.0

				//try
				//{
				//if we do not find it try to check the identifier mapper
				//if (associatedClass.IdentifierMapper == null) return null;

				//StringTokenizer st = new StringTokenizer(propertyName, ".", false);

				//foreach (string element in st)
				//{
				//  if (property == null)
				//  {
				//    property = associatedClass.IdentifierMapper.GetProperty(element);
				//  }
				//  else
				//  {
				//    if (property.IsComposite)
				//      property = ((Component)property.Value).GetProperty(element);
				//    else
				//      return null;
				//  }
				//}
				//}
				//catch (MappingException)
				//{

				return null;
				//}

				#endregion
			}

			return property;
		}

		#region IClassValidatorImplementor Members

		IEnumerable<InvalidValue> IClassValidatorImplementor.GetInvalidValues(object entity, ISet circularityState, ICollection<object> activeTags)
		{
			return GetInvalidValues(entity, circularityState, activeTags);
		}

		IDictionary<System.Type, IClassValidator> IClassValidatorImplementor.ChildClassValidators
		{
			get { return childClassValidators; }
		}

		#endregion

		/// <summary>
		/// Just In Time ClassValidatorFactory
		/// </summary>
		private class JITClassValidatorFactory : AbstractClassValidatorFactory
		{
			private readonly JITClassMappingFactory classMappingFactory;

			public JITClassValidatorFactory(IConstraintValidatorFactory constraintValidatorFactory,
			                                ResourceManager resourceManager, CultureInfo culture,
			                                IMessageInterpolator userInterpolator, ValidatorMode validatorMode,
			                                IEntityTypeInspector entityTypeInspector)
				: base(constraintValidatorFactory, resourceManager, culture, userInterpolator, validatorMode, entityTypeInspector)
			{
				classMappingFactory = new JITClassMappingFactory();
			}

			public override IClassMappingFactory ClassMappingFactory
			{
				get { return classMappingFactory; }
			}

			#region IClassValidatorFactory Members

			public override IClassValidator GetRootValidator(System.Type type)
			{
				return new ClassValidator(type, ConstraintValidatorFactory, new Dictionary<System.Type, IClassValidator>(), this);
			}

			public override void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType)
			{
				new ClassValidator(childType, ConstraintValidatorFactory, parentValidator.ChildClassValidators, this);
			}

			#endregion
		}

		//[OnDeserialized]
		//private void DeserializationCallBack(StreamingContext context)
		//{
		//    userInterpolator = (IMessageInterpolator)Activator.CreateInstance(userInterpolatorType);
			
		//    messageBundle = GetDefaultResourceManager();
		//    defaultMessageBundle = GetDefaultResourceManager();
		//    culture = factory.Culture;
		//    userInterpolator = factory.UserInterpolator;
		//    userInterpolatorType = factory.UserInterpolator.GetType();
		//    this.childClassValidators = childClassValidators;
		//    validatorMode = factory.ValidatorMode;
		//}


		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("interpolator", userInterpolatorType);
			info.AddValue("entityType", entityType);
			info.AddValue("entityValidators", entityValidators);
			info.AddValue("membersToValidate", membersToValidate);
			info.AddValue("childClassValidators", childClassValidators);
			info.AddValue("childGetters", childGetters);
			info.AddValue("defaultInterpolator", defaultInterpolator);
			info.AddValue("membersAttributesDictionary", membersAttributesDictionary);
		}

		public ClassValidator(SerializationInfo info, StreamingContext ctxt)
		{
			var interpolatorType = (System.Type)info.GetValue("interpolator", typeof(System.Type));
			if(interpolatorType != null) userInterpolator = (IMessageInterpolator)Activator.CreateInstance(interpolatorType);
			entityType = (System.Type)info.GetValue("entityType", typeof(System.Type));
			entityValidators = (IList<ValidatorDef>)info.GetValue("entityValidators", typeof(IList<ValidatorDef>));
			membersToValidate = (List<Member>)info.GetValue("membersToValidate", typeof(List<Member>));
			childClassValidators = (IDictionary<System.Type, IClassValidator>)info.GetValue("childClassValidators", typeof(IDictionary<System.Type, IClassValidator>));
			childGetters = (List<MemberInfo>)info.GetValue("childGetters", typeof(List<MemberInfo>));
			membersAttributesDictionary =
				(Dictionary<MemberInfo, List<Attribute>>)
				info.GetValue("membersAttributesDictionary", typeof (Dictionary<MemberInfo, List<Attribute>>));
			defaultMessageBundle = GetDefaultResourceManager();
			messageBundle = GetDefaultResourceManager();
			
			culture = CultureInfo.CurrentCulture;
			defaultInterpolator = (DefaultMessageInterpolatorAggregator)info.GetValue("defaultInterpolator", typeof(DefaultMessageInterpolatorAggregator));
			defaultInterpolator.Initialize(messageBundle,defaultMessageBundle,culture);
		}
	}

	internal static class InvaliValueExtensions
	{
		public static IEnumerable<InvalidValue> WithParent(this IEnumerable<InvalidValue> source, object parentEntity, string memberName)
		{
			foreach (var invalidValue in source)
			{
				invalidValue.AddParentEntity(parentEntity, memberName);
				yield return invalidValue;
			}
		}

		public static IEnumerable<KeyValuePair<object,object>> AsKeyValue(this IEnumerable source)
		{
			var itemsIterator = source.GetEnumerator();
			if (itemsIterator.MoveNext())
			{
				IGetter valueProperty = new BasicPropertyAccessor().GetGetter(itemsIterator.Current.GetType(), "Value");
				IGetter keyProperty = new BasicPropertyAccessor().GetGetter(itemsIterator.Current.GetType(), "Key");
				return source.Cast<object>().Select(
					item => new KeyValuePair<object, object>(keyProperty.Get(item), valueProperty.Get(item)));
			}
			return new KeyValuePair<object, object>[0];
		}
	}
}
