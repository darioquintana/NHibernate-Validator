using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.IoC.Windsor
{
	public class WindsorConstraintValidatorFactory : IConstraintValidatorFactory
	{
		public IValidator GetInstance(Type type)
		{
			return (IValidator) IoC.Container.Resolve(type);
		}

		/// <summary>
		/// Helper to register all the constraints in NHibernate Validator.
		/// This types should be registered as Transient (Each call creates and returns a reference to a new object)
		/// because some of them are statefull.
		/// </summary>
		public static void RegisteringValidators(IWindsorContainer container)
		{
			//Loading NHibernate.Validator.dll assembly.
			Assembly assembly = Assembly.GetAssembly(typeof (ValidatorEngine));

			//Getting the validators to register
			IEnumerable<Type> nativeValidatorsToRegister = assembly.GetTypes().
				Where(x => x.IsClass && !x.IsAbstract &&
				           x.Namespace == "NHibernate.Validator.Constraints" &&
				           x.Name.EndsWith("Validator"));

			foreach (Type type in nativeValidatorsToRegister)
			{
				container.Register(Component.For(type).LifeStyle.Transient);
			}
		}
	}
}