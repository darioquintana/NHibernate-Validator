using System.Web.Mvc;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace MvcNhvDemo.Ext
{
	public static class ValidationExtension
	{
		public static void Validate(this Controller controller, object Entity)
		{
			ValidatorEngine vtor = Environment.SharedEngineProvider.GetEngine();
			InvalidValue[] errors = vtor.Validate(Entity);
			foreach (InvalidValue error in errors)
			{
				controller.ModelState.AddModelError(error.PropertyName, error.Message);
			}
		}
	}
}