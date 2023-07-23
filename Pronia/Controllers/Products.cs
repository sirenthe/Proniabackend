using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
	public class Products : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
