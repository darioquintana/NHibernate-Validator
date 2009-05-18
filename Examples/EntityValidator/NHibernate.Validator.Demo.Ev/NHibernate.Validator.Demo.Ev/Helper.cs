using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Ev
{
	public class Helper
	{
		public static ValidatorEngine Get_Engine_Configured_for_Xml()
		{
			var vtor = new ValidatorEngine();
			var nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = "UseExternal";
			nhvc.Mappings.Add(new MappingConfiguration("NHibernate.Validator.Demo.Ev", null));
			vtor.Configure(nhvc);
			return vtor;
		}

		public static ValidatorEngine Get_Engine_Configured_for_Fluent()
		{
			var vtor = new ValidatorEngine();
			var configuration = new FluentConfiguration();
			configuration
					.Register(Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.Equals("NHibernate.Validator.Demo.Ev.Validators"))
					.ValidationDefinitions())
					.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			vtor.Configure(configuration);
			return vtor;
		}
	}
}