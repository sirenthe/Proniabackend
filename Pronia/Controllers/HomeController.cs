using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;


		public HomeController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var features = _context.Features;	
			var sliders = _context.Sliders;

			HomeViewModel homeViewModel = new HomeViewModel
			{
				Sliders = sliders,
				Features = features
			};
			return View(homeViewModel);
		}
	}
}
