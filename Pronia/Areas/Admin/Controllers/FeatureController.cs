using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    var features= _context.Features;
        //    return View(features);
        //}
        //public IActionResult Detail(int id )
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            var features = await _context.Features.ToListAsync();
            return View(features);
        }


        public async Task<IActionResult> Detail(int id)
        {
            Feature? feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
            if (feature is null)
            {
                return NotFound();
            }
            return View(feature);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
            if (feature is null)
                return NotFound();
            return View(feature);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
            if (feature is null)
                return NotFound();
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

   
