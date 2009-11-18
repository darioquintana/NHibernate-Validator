using System;
using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Engine
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
	public class MyValidatorAttribute : Attribute, IRuleArgs, IValidator
	{
		public MyValidatorAttribute()
		{
			Message = "";
		}
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return false;
		}
		public string Message{get; set;}
	}

	[TestFixture]
	public class AttributeAsValidatorTest
	{
		[MyValidator(Message = "something for class")]
		public class ObjWithEntityValidator
		{
			
		}
		public class ObjWithPropertyValidator
		{
			[MyValidator(Message = "something for prop")]
			public int Value { get; set; }
		}
		
		[Test]
		public void ShouldUseTheValidatorForTheClass()
		{
			var cv = new ClassValidator(typeof (ObjWithEntityValidator));
			var iv = cv.GetInvalidValues(new ObjWithEntityValidator()).ToArray();
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("something for class");
		}

		[Test]
		public void ShouldUseTheValidatorForTheProperty()
		{
			var cv = new ClassValidator(typeof(ObjWithPropertyValidator));
			var iv = cv.GetInvalidValues(new ObjWithPropertyValidator()).ToArray();
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("something for prop");
		}
	}
}