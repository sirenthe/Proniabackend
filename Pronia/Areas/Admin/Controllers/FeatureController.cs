using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels.FeatureViewModels;
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

            var features = await _context.Features.Where(f=> f.IsDeleted==false).ToListAsync();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeatureViewModel createFeatureViewModel)
        {
            
            int featureCount = _context.Features.Count();
            if (featureCount >= 3)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            Feature feature = new Feature
            {
                Title = createFeatureViewModel.Title,
                Description = createFeatureViewModel.Description,
                Image = createFeatureViewModel.Image,

            };

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
        //    if (feature is null)
        //        return NotFound();
        //    return View(feature);
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
        //    if (feature is null)
        //        return NotFound();
        //    int featureCount = _context.Features.Count();
        //    if (featureCount <= 1)
        //    {

        //        return RedirectToAction(nameof(Index));
        //    }
        //    _context.Features.Remove(feature);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

            public async Task<IActionResult> Delete(int id)
        {
            IQueryable<Feature> query = _context.Features.AsQueryable();


            if (await query.Features.CountAsync(f => !f.IsDeleted))
            {
                return BadRequest();
            }

            var feature = await query.Features.FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
            if (feature is null) return NotFound();
            DeleteFeatureViewModel deleteFeatureViewModel = new DeleteFeatureViewModel
            {
                Image = feature.Image,
                Title = feature.Title,

                Description = feature.Description,

            };
            return View();
        }
        [HttpPost
            ]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> DeleteFeature (int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
            if (feature is null) return NotFound();
            feature.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }




        public async Task<IActionResult> Update(int id)
        {
           var feature= await _context.Features.FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
            if(feature is null) return NotFound();
            UpdateFeatureViewModel updateFeatureViewModel = new UpdateFeatureViewModel
            {
                Image = feature.Image,
                id = feature.Id,
                Description = feature.Description,
                Title = feature.Title,
            };
            return View(updateFeatureViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id , UpdateFeatureViewModel updateFeatureViewModel)
        {
            if(!ModelState .IsValid)
            {
                return View();
                
                var feature= await _context.Features.FirstOrDefaultAsync(f=>f.Id== id && !f.IsDeleted);
   if(feature is null) return NotFound();
   feature.Description = updateFeatureViewModel.Description;    
                feature.Title = updateFeatureViewModel.Title;
                feature.Image
                    = updateFeatureViewModel.Image;
                _context.Features.Update(feature);

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
