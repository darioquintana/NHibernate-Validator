using System;
using System.Threading;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ThreadSafe
{
	[TestFixture]
	public class ThreadSafeFixture
	{
		public delegate void Proc();

		private const int ITERATIONS = 1000;
		private static ValidatorEngine ve;

		public class ExceptionHandler
		{
			private int counter;

			public int Count
			{
				get { return counter; }
			}

			public void OnThreadException(object sender, UnhandledExceptionEventArgs e)
			{
				Interlocked.Increment(ref counter);
			}
		}

		public void Run(Proc procediment)
		{
			ExceptionHandler eh = new ExceptionHandler();

			AppDomain.CurrentDomain.UnhandledException += eh.OnThreadException;

			procediment.Invoke();

			AppDomain.CurrentDomain.UnhandledException -= eh.OnThreadException;

			if (eh.Count > 0)
				Assert.Fail("Engine Validator concurrent issues. Concurrent issues count {0}", eh.Count);
		}

		[Test]
		public void AddValidator()
		{
			ve = new ValidatorEngine();

			Run(delegate
			    	{
			    		for (int i = 0; i < ITERATIONS; i++)
			    		{
			    			Thread t = new Thread(
			    				delegate() { ve.AddValidator<Foo>(); });

			    			t.Start();
			    		}
			    	});
		}

		[Test]
		public void IsValid()
		{
			ve = new ValidatorEngine();

			Run(delegate
			{
				for (int i = 0; i < ITERATIONS; i++)
				{
					Thread t = new Thread(delegate() { ve.IsValid(new Foo()); });

					t.Start();
				}
			});
		}

		[Test]
		public void Validate()
		{
			ve = new ValidatorEngine();

			Run(delegate
			{
				for (int i = 0; i < ITERATIONS; i++)
				{
					Thread t = new Thread(delegate() { ve.Validate(new Foo()); });

					t.Start();
				}
			});
		}



	}
}