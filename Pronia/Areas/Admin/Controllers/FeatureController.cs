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

        public async Task<IActionResult> Index()
        {
            var features = await _context.Features.ToListAsync();
            return View(features);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
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
            int featureCount = _context.Features.Count();
            if (featureCount >= 3)
            {
              
                return RedirectToAction(nameof(Index));
            }

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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
            if (feature is null)
                return NotFound();
            int featureCount = _context.Features.Count();
            if (featureCount <= 1)
            {

                return RedirectToAction(nameof(Index));
            }
            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
