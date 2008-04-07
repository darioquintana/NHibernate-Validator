using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Util;

namespace NHibernate.Validator.Util
{
	/// <summary>
	/// Utils methods for type issues.
	/// </summary>
	public sealed class TypeUtils
	{
		/// <summary>
		/// Get the Generic Arguments of a <see cref="IDictionary{TKey,TValue}"/>
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public static KeyValuePair<System.Type, System.Type> GetGenericTypesOfDictionary(MemberInfo member)
		{
			System.Type clazz = GetType(member);

			return new KeyValuePair<System.Type, System.Type>(clazz.GetGenericArguments()[0], clazz.GetGenericArguments()[1]);
		}

		/// <summary>
		/// Get the type of the a Field or Property. 
		/// If is a: Generic Collection or a Array, return the type of the elements.
		/// </summary>
		/// <param name="member">MemberInfo, represent a property or field</param>
		/// <returns>type of the member or collection member</returns>
		public static System.Type GetTypeOfMember(MemberInfo member)
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
		public static bool IsEnumerable(System.Type clazz)
		{
			return clazz.GetInterface(typeof(IEnumerable).FullName) == null ? false : true;
		}

		public static bool IsGenericDictionary(System.Type clazz)
		{
			if (clazz.IsInterface && clazz.IsGenericType)
				return typeof(IDictionary<,>).Equals(clazz.GetGenericTypeDefinition());
			else
				return clazz.GetInterface(typeof(IDictionary<,>).Name) == null ? false : true;
		}

		/// <summary>
		/// Get the <see cref="Type"/> of a <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public static System.Type GetType(MemberInfo member)
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
		/// </summary>
		/// <param name="bean"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		public static object GetMemberValue(object bean, MemberInfo member)
		{
			FieldInfo fi = member as FieldInfo;
			if (fi != null)
				return fi.GetValue(bean);

			PropertyInfo pi = member as PropertyInfo;
			if (pi != null)
				return pi.GetValue(bean, ReflectHelper.AnyVisibilityInstance | BindingFlags.GetProperty, null, null, null);

			return null;
		}

		public static MemberInfo GetPropertyOrField(System.Type currentClass, string name)
		{
			MemberInfo memberInfo = currentClass.GetProperty(name, ReflectHelper.AnyVisibilityInstance | BindingFlags.Static);
			if (memberInfo == null)
			{
				memberInfo = currentClass.GetField(name, ReflectHelper.AnyVisibilityInstance | BindingFlags.Static);
			}

			return memberInfo;
		}

		/// <summary>
		/// Tell if a type implements a interface.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="interfaceType">Implements this interface?</param>
		/// <returns>
		/// True if the type implements the interface and 
		/// false if the doesn't or the interfaceType param isn't an interface
		/// </returns>
		public static bool IsImplementationOf(System.Type type, System.Type interfaceType)
		{
			if(!interfaceType.IsInterface)
			{
				return false;
			}
			else
			{
				foreach (System.Type @interface in type.GetInterfaces())
				{
					if (@interface.Equals(interfaceType)) 
						return true;
				}
				return false;
			}
		}
	}
}