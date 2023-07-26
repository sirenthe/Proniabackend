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
        //INDEX
        public async Task<IActionResult> Index()
        {

            var features = await _context.Features.Where(f=> f.IsDeleted==false).ToListAsync();
            return View(features);
        }





        //CREATE
        public async Task<IActionResult> Create()

        {
            if(await _context.Features.CountAsync()==3) {
                return BadRequest();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeatureViewModel createFeatureViewModel)
        {
            //    return Content(createFeatureViewModel.Image.FileName);
            //return Content(createFeatureViewModel.Image.ContentType);
            //return Content(createFeatureViewModel.Image.Length.ToString());
            if (await _context.Features.CountAsync() == 3)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            string path = "C:\\Users\\Windows\\Desktop\\Task\\Pronia\\Pronia\\wwwroot\\assets\\images\\website-images\\" + createFeatureViewModel.Image.FileName;
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await createFeatureViewModel.Image.CopyToAsync(fileStream);
            }
             
            //fileStream.Dispose();


            Feature feature = new Feature
            {
                Title = createFeatureViewModel.Title,
                Description = createFeatureViewModel.Description,
                Image = createFeatureViewModel.Image.FileName,


            };

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



   //UPDATE UPDATE
        public async Task<IActionResult> Update(int id)  {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var feature = await _context.Features.FirstOrDefaultAsync(f=> f.Id == id);
            if(feature == null)
            {
                return NotFound();
            }
            UpdateFeatureViewModel updateFeatureViewModel = new UpdateFeatureViewModel
            {
                Title = feature.Title,
                Description = feature.Description,
                Image = feature.Image,
                Id = feature.Id
            };
            return View(updateFeatureViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update (int id, UpdateFeatureViewModel updateFeatureViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var feature = await _context.Features.FirstOrDefaultAsync(f=>f.Id== id);
            if(feature == null)
            {
                return NotFound();
            }
            feature.Title = updateFeatureViewModel.Title;
            feature.Image = updateFeatureViewModel.Image;
            feature.Description = updateFeatureViewModel.Description;
            _context.Features.Update(feature);
            await _context.SaveChangesAsync();

      
            return RedirectToAction(nameof(Index));
        }








        //DELETE
public async  Task<IActionResult> delete (int id)
        {
            IQueryable<Feature> query =_context.Features.AsQueryable();
if(await query.CountAsync()==1)
                return BadRequest();
            var feature = await query.FirstOrDefaultAsync(f=> f.Id== id );
            if(feature == null)
            {
                return NotFound();
            }
            DeleteFeatureViewModel deleteFeatureViewModel = new DeleteFeatureViewModel()
            {
                Image = feature.Image,
                Description = feature.Description,
                Title = feature.Title,

            };
            return View(deleteFeatureViewModel);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deletefeature(int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id );
            if (feature == null)
            {
                return NotFound();
            }
            feature.IsDeleted = true;
          await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // DETAIL
        public async Task<IActionResult> Detail(int id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
            if (feature is null)
            {
                return NotFound();
            }
            return View(feature);
        }
    }

}




//IgnoreQueryFilters() ile ise bu hasfilteri ignore edirik 