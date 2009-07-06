using Castle.Windsor;

namespace NHibernate.Validator.Demo.IoC.Windsor
{
	/// <summary>
	/// You should adapt this class to your needs.
	/// The idea of this class is let the IoC Container to be accessible
	/// from everywhere in the Application.
	/// In a Web application you might want to implement <see cref="IContainerAccessor"/>
	/// in your HttpApplication (in the Global.asax.cs) then the container can be accessed from
	/// everywhere via: HttpContext.Current.ApplicationInstance
	/// </summary>
	public class IoC : IContainerAccessor
	{
		public static IWindsorContainer Container { get; set; }
        
		IWindsorContainer IContainerAccessor.Container
		{
			get { return Container; }
		}
	}
}