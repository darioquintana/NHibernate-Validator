using System.Web.Mvc;
using MvcNhvDemo.Ext;
using MvcNhvDemo.Models;

namespace MvcNhvDemo.Controllers
{
	public class CustomerController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Manage()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Manage(Customer customer)
		{
			this.Validate(customer);

			if (ModelState.IsValid)
			{
				//Do something with the valid entity.
				return View();
			}
			else
				return View(customer);
		}
	}
}