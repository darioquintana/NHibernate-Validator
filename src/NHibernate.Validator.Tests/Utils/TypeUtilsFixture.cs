using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Util;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Utils
{
	public class BaseTestingClass
	{
		public string bSimpleStr;

		public int BaseIntProp
		{
			get { return 37; }
		}
	}

	public class TestingClass:BaseTestingClass
	{
		public IDictionary<string, int> dic;
		public IList<double> list;
		public int[] intArray;
		public string simpleStr;
		public IList noGenericList;

		public int IntProp
		{
			get { return 31; }
		}

		public string Problem
		{
			get { throw new Exception(); }
		}
	}

	[TestFixture]
	public class TypeUtilsFixture
	{
		[Test]
		public void GetGenericTypesOfDictionary()
		{
			MemberInfo member = typeof (TestingClass).GetField("dic");
			KeyValuePair<System.Type, System.Type> p = TypeUtils.GetGenericTypesOfDictionary(member);
			Assert.AreEqual(typeof (string), p.Key);
			Assert.AreEqual(typeof(int), p.Value);
		}

		[Test]
		public void GetTypeOfMember()
		{
			MemberInfo member = typeof(TestingClass).GetField("list");
			Assert.AreEqual(typeof(double), TypeUtils.GetTypeOfMember(member));

			member = typeof(TestingClass).GetField("intArray");
			Assert.AreEqual(typeof(int), TypeUtils.GetTypeOfMember(member));

			member = typeof(TestingClass).GetField("simpleStr");
			Assert.AreEqual(typeof(string), TypeUtils.GetTypeOfMember(member));

			member = typeof(TestingClass).GetField("noGenericList");
			Assert.AreEqual(typeof(IList), TypeUtils.GetTypeOfMember(member));
		}

		[Test]
		public void IsGenericDictionary()
		{
			Assert.IsTrue(TypeUtils.IsGenericDictionary(typeof (IDictionary<string, int>)));
			Assert.IsFalse(TypeUtils.IsGenericDictionary(typeof(IList<string>)));
			Assert.IsTrue(TypeUtils.IsGenericDictionary(typeof(Dictionary<string, int>)));
			Assert.IsFalse(TypeUtils.IsGenericDictionary(typeof(List<string>)));
			Assert.IsFalse(TypeUtils.IsGenericDictionary(typeof(int)));
		}

		[Test]
		public void GetMemberType()
		{
			MemberInfo member = typeof(TestingClass).GetField("simpleStr");
			Assert.AreEqual(typeof(string), TypeUtils.GetType(member));

			member = typeof(TestingClass).GetProperty("IntProp");
			Assert.AreEqual(typeof(int), TypeUtils.GetType(member));

			member = typeof(TestingClass).GetMethod("ToString");
			try
			{
				TypeUtils.GetType(member);
				Assert.Fail("Accept method");
			}
			catch(ArgumentException)
			{
				// ok
			}
		}

		[Test]
		public void GetMemberValue()
		{
			TestingClass tc = new TestingClass();
			tc.bSimpleStr = "BaseValue";
			tc.simpleStr = "aValue";
			MemberInfo fieldMember = typeof(TestingClass).GetField("simpleStr");
			MemberInfo propMember = typeof(TestingClass).GetProperty("IntProp");

			MemberInfo baseFieldMember = typeof(BaseTestingClass).GetField("bSimpleStr");
			MemberInfo basePropMember = typeof(BaseTestingClass).GetProperty("BaseIntProp");

			Assert.AreEqual("aValue", TypeUtils.GetMemberValue(tc, fieldMember));
			Assert.AreEqual(31, TypeUtils.GetMemberValue(tc, propMember));
			Assert.AreEqual("BaseValue", TypeUtils.GetMemberValue(tc, baseFieldMember));
			Assert.AreEqual(37, TypeUtils.GetMemberValue(tc, basePropMember));

			// the null value is used in ChildValidation
			// we don't take care if, for some reason, we are looking for a value of something else than a field or property
			MemberInfo methodMember = typeof(TestingClass).GetMethod("ToString");
			Assert.IsNull(TypeUtils.GetMemberValue(tc, methodMember));
		}

		[Test, ExpectedException(typeof(InvalidStateException))]
		public void GetterWithException()
		{
			TestingClass tc = new TestingClass();
			MemberInfo propMember = typeof(TestingClass).GetProperty("Problem");
			TypeUtils.GetMemberValue(tc, propMember);
		}

		[Test]
		public void GetPropertyOrField()
		{
			Assert.IsNotNull(TypeUtils.GetPropertyOrField(typeof (TestingClass), "simpleStr"));
			Assert.IsNotNull(TypeUtils.GetPropertyOrField(typeof(TestingClass), "IntProp"));
			Assert.IsNotNull(TypeUtils.GetPropertyOrField(typeof(TestingClass), "bSimpleStr"));
			Assert.IsNotNull(TypeUtils.GetPropertyOrField(typeof(TestingClass), "BaseIntProp"));
			Assert.IsNull(TypeUtils.GetPropertyOrField(typeof(TestingClass), "WrongName"));
		}
	}
}
